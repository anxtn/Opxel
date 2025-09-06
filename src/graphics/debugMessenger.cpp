#include "debugMessenger.hpp"
#include "../logger.hpp"
#include "toStringUtils.hpp"
#include <format>
#include <stdexcept>

VkResult vkCreateDebugUtilsMessengerEXT(
    VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT *pCreateInfo,
    const VkAllocationCallbacks *pAllocator,
    VkDebugUtilsMessengerEXT *pMessenger) {
    auto func = (PFN_vkCreateDebugUtilsMessengerEXT)vkGetInstanceProcAddr(
        instance, "vkCreateDebugUtilsMessengerEXT");
    if (func != nullptr) {
        return func(instance, pCreateInfo, pAllocator, pMessenger);
    } else {
        return VK_ERROR_EXTENSION_NOT_PRESENT;
    }
}

void vkDestroyDebugUtilsMessengerEXT(VkInstance instance,
                                     VkDebugUtilsMessengerEXT messenger,
                                     const VkAllocationCallbacks *pAllocator) {
    auto func = (PFN_vkDestroyDebugUtilsMessengerEXT)vkGetInstanceProcAddr(
        instance, "vkDestroyDebugUtilsMessengerEXT");
    if (func != nullptr) {
        func(instance, messenger, pAllocator);
    }
}

namespace graphics {
DebugMessenger::DebugMessenger(Instance *instance,
                               PFN_vkDebugUtilsMessengerCallbackEXT callback) {

    assert(instance != nullptr);

    instance_ = instance;
    VkDebugUtilsMessengerCreateInfoEXT createInfo = {};
    createInfo.sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT;
    createInfo.messageSeverity =
        VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT |
        VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT |
        VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT;
    createInfo.messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT |
                             VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT |
                             VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT;
    createInfo.pfnUserCallback = callback;
    VkResult res =
        vkCreateDebugUtilsMessengerEXT(instance->getVkInstance(), &createInfo,
                                       nullptr, &vulkanDebugMessenger_);
    if (res != VK_SUCCESS) {
        auto errMsg = graphics::VulkanResultToStr(res);
        std::string msg =
            std::format("Failed to create debug messenger: {})", errMsg);
        throw std::runtime_error(msg);
    } else {
        logging::debug("Created DebugMessenger");
    }
}

DebugMessenger::~DebugMessenger() {
    vkDestroyDebugUtilsMessengerEXT(instance_->getVkInstance(),
                                    vulkanDebugMessenger_, nullptr);
    logging::debug("Destroyed Debug Messenger");
}
} // namespace graphics
