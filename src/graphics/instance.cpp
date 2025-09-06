#include "instance.hpp"
#include "../logger.hpp"
#include "toStringUtils.hpp"
#include <format>

namespace graphics {
Instance::Instance(Window *window) {
    assert(window != nullptr);

    VkApplicationInfo appInfo{};
    appInfo.sType = VK_STRUCTURE_TYPE_APPLICATION_INFO;
    appInfo.pApplicationName = "Opxel";
    appInfo.applicationVersion = VK_MAKE_VERSION(1, 0, 0);
    appInfo.pEngineName = "Opxel Engine";
    appInfo.engineVersion = VK_MAKE_VERSION(1, 0, 0);
    appInfo.apiVersion = VK_API_VERSION_1_0;

    const std::vector<const char *> layers = {"VK_LAYER_KHRONOS_validation"};
    uint32_t layerCount = static_cast<uint32_t>(layers.size());

    auto extensions = window->getGLFWVulkanExtensions();
    extensions.push_back(VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
    uint32_t extensionCount = static_cast<uint32_t>(extensions.size());

    VkInstanceCreateInfo createInfo{};
    createInfo.sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
    createInfo.pApplicationInfo = &appInfo;
    createInfo.enabledExtensionCount = extensionCount;
    createInfo.ppEnabledExtensionNames = extensions.data();
    createInfo.enabledLayerCount = layerCount;
    createInfo.ppEnabledLayerNames = layers.data();
    createInfo.pNext = nullptr;

    VkResult res = vkCreateInstance(&createInfo, nullptr, &vulkanInstance_);
    if (res != VK_SUCCESS) {
        auto errMsg = VulkanResultToStr(res);
        auto msg = std::format("Failed to create Vulkan instance: {}", errMsg);
        throw std::runtime_error(msg);
    } else if (vulkanInstance_) {
        auto msg = std::format("Created Vulkan instance ({:#X})",
                               reinterpret_cast<uintptr_t>(vulkanInstance_));
        logging::debug(msg);
    }
}

VkInstance Instance::getVkInstance() const {
    assert(vulkanInstance_ != nullptr);
    return vulkanInstance_;
}

std::vector<PhysicalDeviceInfo> Instance::getAvailablePhysicalDevices() const {
    uint32_t deviceCount = 0;
    vkEnumeratePhysicalDevices(vulkanInstance_, &deviceCount, nullptr);
    VkPhysicalDevice *devices = new VkPhysicalDevice[deviceCount];
    VkResult res = vkEnumeratePhysicalDevices(vulkanInstance_, &deviceCount, devices);
    std::vector<PhysicalDeviceInfo> deviceInfos;
    deviceInfos.reserve(deviceCount);
    for (size_t i = 0; i < deviceCount; i++) {
        deviceInfos.push_back(PhysicalDeviceInfo(devices[i]));
    }

    if (res != VK_SUCCESS) {
        auto msg =
            std::format("Failed to enumerate physical devices: {}", VulkanResultToStr(res));
        logging::error(msg);
        return {};
    }

    return deviceInfos;
}

Instance::~Instance() {
    vkDestroyInstance(vulkanInstance_, nullptr);
    auto msg = std::format("Destroyed Vulkan instance ({})",
                           reinterpret_cast<uintptr_t>(vulkanInstance_));
    logging::debug(msg);
}
} // namespace graphics
