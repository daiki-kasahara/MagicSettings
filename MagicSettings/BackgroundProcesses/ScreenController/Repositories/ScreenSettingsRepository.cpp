#include "ScreenSettingsRepository.h"
#include <filesystem>
#include <fstream>
#include <sstream>
#include <nlohmann/json.hpp>

using namespace ScreenController::Repositories;

ScreenSettingsRepository::ScreenSettingsRepository()
{
    FilePath = std::filesystem::current_path().generic_string();
}

std::string ReadFileToString(const std::string& filename) {

    std::ifstream file(filename);

    if (!file.is_open()) {
        return "";
    }

    // ファイルの末尾まで移動して、サイズを取得する
    file.seekg(0, std::ios::end);
    std::streampos fileSize = file.tellg();
    file.seekg(0, std::ios::beg);

    // ファイルサイズに合わせてstringをリサイズして読み込む
    std::string content;
    content.resize(fileSize);

    // ファイル全体を一括で読み込む
    file.read(&content[0], fileSize);

    file.close();
    return content;
}

auto ScreenSettingsRepository::Get() -> BlueLightBlockingFilter
{
    nlohmann::json json;
    try
    {
        json = nlohmann::json::parse(ReadFileToString(FilePath));
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
