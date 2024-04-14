#pragma once
#include <magnification.h>

namespace ScreenController::Services {
    enum class BlueLightBlockingFilter
    {
        None,
        Ten,
        Twenty,
        Thirty,
        Forty,
        Fifty,
        Sixty,
        Seventy,
        Eighty,
        Ninety,
        OneHandled,
    };

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
