#include "logger.hpp"
#include <spdlog/sinks/stdout_color_sinks.h>
#include <spdlog/spdlog.h>

namespace logging {
void Init() {
  auto console = spdlog::stdout_color_mt("console");
  spdlog::set_default_logger(console);
  spdlog::set_pattern("[%T] [%^%l%$] %v");
  spdlog::set_level(spdlog::level::trace);
}

void trace(std::string message) { spdlog::trace(message); }

void debug(std::string message) { spdlog::debug(message); }

void info(std::string message) { spdlog::info(message); }

void warn(std::string message) { spdlog::warn(message); }

void error(std::string message) { spdlog::error(message); }

void critical(std::string message) { spdlog::critical(message); }
} // namespace logging
