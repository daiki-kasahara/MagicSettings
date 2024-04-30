#pragma once

#include "BlueLightBlockingFilter.h"

using namespace ScreenController::Domains;

namespace ScreenController::Services {

    /// <summary>
    /// �X�N���[���Ƀt�B���^��������N���X
    /// </summary>
    class ScreenFilter
    {
    public:
        ScreenFilter() {};
        ~ScreenFilter();

    public:
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        auto Initialize() noexcept -> bool;

        /// <summary>
        /// �I������
        /// </summary>
        /// <returns></returns>
        auto Uninitialize() noexcept -> void;

        /// <summary>
        /// �t�B���^��ݒ肷��
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        auto Set(BlueLightBlockingFilter filter) noexcept -> bool;
    };
}
