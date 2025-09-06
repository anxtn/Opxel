#include "physicalDevicePicker.hpp"

namespace graphics {

PhysicalDevicePicker::PhysicalDevicePicker(const PhysicalDeviceRequirements &requirements)
    : requirements_(requirements) {
}

bool PhysicalDevicePicker::isPhysicalDeviceSuitable(const PhysicalDeviceInfo &deviceInfo) const {

    bool hasMinimumApiVersion = deviceInfo.props.apiVersion >= requirements_.minimumApiVersion;
    bool hasGeomShader = deviceInfo.features.geometryShader;

    return hasMinimumApiVersion && hasGeomShader;
}

int PhysicalDevicePicker::evaluateScore(const PhysicalDeviceInfo &deviceInfo) const {
    const auto &properties = deviceInfo.props;
    const auto &features = deviceInfo.features;

    if (!features.geometryShader)
        return 0;

    int score = 0;

    switch (properties.deviceType) {
    case VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU:
        score += 950;
        break;
    case VK_PHYSICAL_DEVICE_TYPE_VIRTUAL_GPU:
        score += 550;
        break;
    case VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU:
        score += 400;
        break;
    case VK_PHYSICAL_DEVICE_TYPE_CPU:
        score += 100;
        break;
    default:
        break;
    }

    score += properties.limits.maxImageDimension2D;

    return score;
}
} // namespace graphics
