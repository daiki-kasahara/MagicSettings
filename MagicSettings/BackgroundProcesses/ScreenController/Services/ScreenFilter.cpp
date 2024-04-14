#include "ScreenFilter.h"

using namespace ScreenController::Services;

ScreenFilter::~ScreenFilter()
{
    Uninitialize();
}

auto ScreenFilter::Initialize() noexcept -> bool
{
    return MagInitialize();
}

auto ScreenFilter::Uninitialize() noexcept -> void
{
    MagUninitialize();
}

auto ScreenFilter::Set(BlueLightBlockingFilter filter) noexcept -> void
{
    auto value = 1.0f;

    switch (filter)
    {
    case BlueLightBlockingFilter::None:
        value = 1.0f;
        break;
    case BlueLightBlockingFilter::Ten:
        value = 0.9f;
        break;
    case BlueLightBlockingFilter::Twenty:
        value = 0.8f; 
        break;
    case BlueLightBlockingFilter::Thirty:
        value = 0.7f;
        break;
    case BlueLightBlockingFilter::Forty:
        value = 0.6f;
        break;
    case BlueLightBlockingFilter::Fifty:
        value = 0.5f;
        break;
    case BlueLightBlockingFilter::Sixty:
        value = 0.4f;
        break;
    case BlueLightBlockingFilter::Seventy:
        value = 0.3f;
        break;
    case BlueLightBlockingFilter::Eighty:
        value = 0.2f;
        break;
    case BlueLightBlockingFilter::Ninety:
        value = 0.1f;
        break;
    case BlueLightBlockingFilter::OneHundred:
        value = 0.0f;
        break;
    default:
        break;
    }

    auto MagEffectNormal = MAGCOLOREFFECT{ 1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                                                 0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
                                                 0.0f,  0.0f,  value,  0.0f,  0.0f,
                                                 0.0f,  0.0f,  0.0f,  1.0f,  0.0f,
                                                 0.0f,  0.0f,  0.0f,  0.0f,  1.0f };

    MagSetFullscreenColorEffect(&MagEffectNormal);
}