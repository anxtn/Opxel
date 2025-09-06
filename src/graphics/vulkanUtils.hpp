#pragma once
#include <vector>
#include <vulkan/vulkan.h>
namespace graphics {
[[nodiscard]]
std::vector<VkExtensionProperties> GetAvailableExtensions();
[[nodiscard]]
std::vector<VkLayerProperties> GetAvailableLayers();
[[nodiscard]]
std::vector<VkPhysicalDevice> GetAvailablePhysicalDevices(VkInstance instance);
} // namespace graphics
