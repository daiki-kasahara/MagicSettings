#pragma once

#include "BlueLightBlockingFilter.h"

using namespace ScreenController::Domains;

namespace ScreenController::Services {

    /// <summary>
    /// スクリーンにフィルタをかけるクラス
    /// </summary>
    class ScreenFilter
    {
    public:
        ScreenFilter() {};
        ~ScreenFilter();

    public:
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns></returns>
        auto Initialize() noexcept -> bool;

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <returns></returns>
        auto Uninitialize() noexcept -> void;

        /// <summary>
        /// フィルタを設定する
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        auto Set(BlueLightBlockingFilter filter) noexcept -> bool;
    };
}
