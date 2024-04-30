#pragma once

#include "BlueLightBlockingFilter.h"
#include <string>

using namespace ScreenController::Domains;

namespace ScreenController::Repositories
{
    /// <summary>
    /// スクリーンのリポジトリ
    /// </summary>
    class ScreenSettingsRepository
    {
    public:
        /// <summary>
        /// 設定を取得する
        /// </summary>
        /// <returns></returns>
        auto Get() -> BlueLightBlockingFilter;

        /// <summary>
        /// 設定する
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        auto Set(bool isEnabled) -> void;
    };
}
