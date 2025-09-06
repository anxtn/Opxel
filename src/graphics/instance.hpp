#pragma once
#include "physicalDeviceInfo.hpp"
#include "window.hpp"
#include <vulkan/vulkan.h>

namespace graphics {
class Instance {
  public:
    Instance(Window *window);
    ~Instance();
    [[nodiscard]]
    VkInstance getVkInstance() const;
    [[nodiscard]]
    std::vector<PhysicalDeviceInfo> getAvailablePhysicalDevices() const;

  private:
    VkInstance vulkanInstance_;
};
} // namespace graphics
