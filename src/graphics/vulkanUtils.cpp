#include "vulkanUtils.hpp"
#include "../logger.hpp"
#include "toStringUtils.hpp"
#include <format>

namespace graphics {

std::vector<VkExtensionProperties> GetAvailableExtensions() {
    uint32_t extensionCount = 0;
    vkEnumerateInstanceExtensionProperties(nullptr, &extensionCount, nullptr);
    std::vector<VkExtensionProperties> extensions(extensionCount);
    VkResult res = vkEnumerateInstanceExtensionProperties(
        nullptr, &extensionCount, extensions.data());

    if (res != VK_SUCCESS) {
        auto errStr = graphics::VulkanResultToStr(res);
        auto msg = std::format("Failed to enumerate extensions : {}", errStr);
        logging::error(msg);
        return {};
    }

    return extensions;
}

std::vector<VkLayerProperties> GetAvailableLayers() {
    uint32_t layerCount = 0;
    vkEnumerateInstanceLayerProperties(&layerCount, nullptr);
    std::vector<VkLayerProperties> layers(layerCount);
    VkResult res =
        vkEnumerateInstanceLayerProperties(&layerCount, layers.data());

    if (res != VK_SUCCESS) {
        auto errStr = VulkanResultToStr(res);
        auto msg =
            std::format("Failed to enumerate instance layers: {}", errStr);
        logging::error(msg);
        return {};
    }

    return layers;
}

} // namespace graphics
