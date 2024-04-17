#include "ScreenSettingsRepository.h"
#include <windows.h>
#include <fstream>
#include <format>
#include <nlohmann/json.hpp>

using namespace ScreenController::Repositories;

std::string GetFilePath()
{

    char path[MAX_PATH];
    if (!GetModuleFileNameA(NULL, path, MAX_PATH) != 0)
        return "";

    std::string fullPath(path);
    size_t lastSlashPos = fullPath.find_last_of("\\");
    if (lastSlashPos == std::string::npos)
        return "";

    return std::format("{}\\Settings\\screen.json", fullPath.substr(0, lastSlashPos));
}

auto ScreenSettingsRepository::Get() -> BlueLightBlockingFilter
{
    nlohmann::json json;
    try
    {
        json = nlohmann::json::parse(std::ifstream(GetFilePath()));
    }
    catch (const std::exception&)
    {
        return BlueLightBlockingFilter::None;
    }

    if (json.is_null())
        return BlueLightBlockingFilter::None;

    try
    {
        return static_cast<BlueLightBlockingFilter>(json["BlueLightBlocking"].get<int>());
    }
    catch (const std::exception&)
    {
        return BlueLightBlockingFilter::None;
    }
}
