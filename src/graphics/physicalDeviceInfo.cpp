#include "physicalDeviceInfo.hpp"
#include "toStringUtils.hpp"
#include <format>
#include <glm/ext/scalar_uint_sized.hpp>

std::vector<VkQueueFamilyProperties> getQueueFamilyProps(VkPhysicalDevice device) {
    uint32_t queueFamilyCount = 0;
    vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, nullptr);
    std::vector<VkQueueFamilyProperties> queueFamilies(queueFamilyCount);
    vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyCount, queueFamilies.data());

    return queueFamilies;
}

VkPhysicalDeviceProperties getPhysicalDeviceProps(VkPhysicalDevice device) {
    VkPhysicalDeviceProperties properties;
    vkGetPhysicalDeviceProperties(device, &properties);
    return properties;
}

VkPhysicalDeviceFeatures getPhysicalDeviceFeatures(VkPhysicalDevice device) {
    VkPhysicalDeviceFeatures features;
    vkGetPhysicalDeviceFeatures(device, &features);
    return features;
}

VkPhysicalDeviceMemoryProperties getMemoryProps(VkPhysicalDevice device) {
    VkPhysicalDeviceMemoryProperties props;
    vkGetPhysicalDeviceMemoryProperties(device, &props);
    return props;
}

uint64_t getVRAMSize(const VkPhysicalDeviceMemoryProperties &memProps) {
    uint64_t size = 0;
    for (auto i = 0; i < memProps.memoryHeapCount; i++) {
        const VkMemoryHeap &heap = memProps.memoryHeaps[i];
        if (heap.flags & VK_MEMORY_HEAP_DEVICE_LOCAL_BIT) {
            size += memProps.memoryHeaps[i].size;
        }
    }

    return size;
}

namespace graphics {

PhysicalDeviceInfo::
    PhysicalDeviceInfo(VkPhysicalDevice device)
    : device(device),
      props(getPhysicalDeviceProps(device)),
      features(getPhysicalDeviceFeatures(device)),
      memoryProps(getMemoryProps(device)),
      queueFamProps(getQueueFamilyProps(device)) {
}

std::string MemSizeToStr(uint64_t memSize) {
    constexpr double KiB = 1024.0;
    constexpr double MiB = 1024.0 * KiB;
    constexpr double GiB = 1024.0 * MiB;

    if (memSize >= GiB)
        return std::format("{:.2f} GiB", memSize / GiB);
    else if (memSize >= MiB)
        return std::format("{:.2f} MiB", memSize / MiB);
    else if (memSize >= KiB)
        return std::format("{:.2f} KiB", memSize / KiB);
    else
        return std::format("{} B", memSize);
}

std::string PhysicalDeviceInfo::getInfoStr() const {
    auto deviceTypeStr = VulkanPhysicalDeviceTypeToStr(props.deviceType);
    auto apiVersionStr = vulkanVersionToStr(props.apiVersion);
    auto driverVersionStr = vulkanVersionToStr(props.driverVersion);
    auto vramSizeStr = MemSizeToStr(getVRAMSize(memoryProps));

    return std::format(
        R"(### Physical Device Info ###
===== General Properties ===
        Device Id:      {}
        Device Name:    {}
        Device Type:    {}
        Api Version:    {}
        Driver Version: {}
        Vendor Id:      {}
===== Memory Properties ====
        Heap Count:      {}
        Total VRAM Size: {}
===== Features =============
        Has Geom Shader: {})",
        props.deviceID,
        props.deviceName,
        deviceTypeStr,
        apiVersionStr,
        driverVersionStr,
        props.vendorID,
        memoryProps.memoryHeapCount,
        vramSizeStr,
        features.geometryShader);
}
} // namespace graphics
