using System;
using System.Collections.Generic;
using System.Linq;

namespace Behaviours.Configs
{
    public class BehaviourConfig<TFeature> where TFeature : Enum
    {
        private readonly HashSet<TFeature> _enabledFeatures = new HashSet<TFeature>();

        public BehaviourConfig() {}

        public BehaviourConfig(TFeature[] initialSettings)
        {
            initialSettings.All(f => _enabledFeatures.Add(f));
        }

        public void Enable(TFeature feature)
        {
            _enabledFeatures.Add(feature);
        }

        public void Disable(TFeature feature)
        {
            _enabledFeatures.Remove(feature);
        }

        public bool IsEnabled(TFeature feature)
        {
            return _enabledFeatures.Contains(feature);
        }
    }
}
