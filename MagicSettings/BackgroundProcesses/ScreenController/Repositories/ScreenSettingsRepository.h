#pragma once

#include "BlueLightBlockingFilter.h"
#include <string>

using namespace ScreenController::Domains;

namespace ScreenController::Repositories
{
    class ScreenSettingsRepository
    {
    public:
        auto Get() -> BlueLightBlockingFilter;

        auto Set(bool isEnabled) -> void;
    };
}
