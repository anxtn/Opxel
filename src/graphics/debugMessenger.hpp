#pragma once
#include "instance.hpp"
#include <vulkan/vulkan.h>

namespace graphics {
class DebugMessenger {
public:
  DebugMessenger(graphics::Instance *instance,
                 PFN_vkDebugUtilsMessengerCallbackEXT callback);
  ~DebugMessenger();

private:
  VkDebugUtilsMessengerEXT vulkanDebugMessenger_;
  graphics::Instance *instance_;
};
} // namespace graphics
