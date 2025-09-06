#pragma once
#include <string>

namespace logging {
void Init();
void trace(std::string message);

void debug(std::string message);

void info(std::string message);

void warn(std::string message);

void error(std::string message);

void critical(std::string message);

} // namespace logging
