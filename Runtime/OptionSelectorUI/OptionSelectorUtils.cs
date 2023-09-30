using System;

namespace OptionSelectorUI {

    public class OptionSelectorUtils {
        public class OnItemSelectedArgs<T> : EventArgs {
            public T Id;
        }
    }
}
