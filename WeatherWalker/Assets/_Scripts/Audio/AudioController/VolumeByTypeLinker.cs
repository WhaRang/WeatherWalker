public static class VolumeByTypeLinker
{
    private static readonly float DEFAULT_VOLUME_LEVEL = 1.0f;

    public static float GetVolumeByType(VolumeType type)
    {
        switch (type)
        {
            // TODO
            case VolumeType.ST_Volume:
                return 1.0f;

            //TODO
            case VolumeType.SFX_Volume:
                return 1.0f;

            default:
                return DEFAULT_VOLUME_LEVEL;
        }
    }
}
