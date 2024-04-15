#pragma once

#include "BlueLightBlockingFilter.h"

using namespace ScreenController::Domains;

namespace ScreenController::Services {

    class ScreenFilter
    {
    public:
        ScreenFilter() {};
        ~ScreenFilter();

    public:
        auto Initialize() noexcept -> bool;
        auto Uninitialize() noexcept -> void;
        auto Set(BlueLightBlockingFilter filter) noexcept -> void;
    };
}
