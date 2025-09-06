#include "renderer.hpp"
#include "../logger.hpp"
#include "physicalDeviceInfo.hpp"
#include <string>

static VKAPI_ATTR VkBool32 VKAPI_CALL
debugCallback(VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
              VkDebugUtilsMessageTypeFlagsEXT messageType,
              const VkDebugUtilsMessengerCallbackDataEXT *pCallbackData,
              void *pUserData) {
    switch (messageSeverity) {
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT:
        logging::debug(pCallbackData->pMessage);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT:
        logging::info(pCallbackData->pMessage);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT:
        logging::warn(pCallbackData->pMessage);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT:
        logging::error(pCallbackData->pMessage);
        break;
    default:
        logging::info(pCallbackData->pMessage);
        break;
    }
    return VK_FALSE;
}

namespace graphics {
Renderer::Renderer(graphics::Window *window)
    : instance_(std::make_unique<graphics::Instance>(window)),
      debugMessenger_(std::make_unique<graphics::DebugMessenger>(
          instance_.get(), debugCallback)) {
    logging::debug("Created Renderer");

    auto devices = instance_.get()->getAvailablePhysicalDevices();

    for (auto &device : devices) {
        auto deviceInfo = graphics::PhysicalDeviceInfo(device);
        logging::info("\n" + deviceInfo.getInfoStr() + "\n\n");
    }
}
Renderer::~Renderer() {
    logging::debug("Destroyed Renderer");
}

} // namespace graphics
