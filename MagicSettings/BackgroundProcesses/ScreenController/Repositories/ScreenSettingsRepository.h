#pragma once

#include "BlueLightBlockingFilter.h"
#include <string>

using namespace ScreenController::Domains;

namespace ScreenController::Repositories
{
    class ScreenSettingsRepository
    {
    private:
        std::string FilePath;

    public:
        ScreenSettingsRepository();
        ~ScreenSettingsRepository() {};

    public:
        auto Get() -> BlueLightBlockingFilter;
    };
}
