#pragma once

#include "BlueLightBlockingFilter.h"
#include <string>

using namespace ScreenController::Domains;

namespace ScreenController::Repositories
{
    /// <summary>
    /// �X�N���[���̃��|�W�g��
    /// </summary>
    class ScreenSettingsRepository
    {
    public:
        /// <summary>
        /// �ݒ���擾����
        /// </summary>
        /// <returns></returns>
        auto Get() -> BlueLightBlockingFilter;

        /// <summary>
        /// �ݒ肷��
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        auto Set(bool isEnabled) -> void;
    };
}
