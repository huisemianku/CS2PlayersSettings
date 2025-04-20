if (!lemonade && typeof (require) === 'function') {
    var lemonade = require('lemonadejs');
}

; (function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
    typeof define === 'function' && define.amd ? define(factory) :
    global.Switch = factory();
}(this, (function () {

    const Switch = function () {
        let self = this;

        const onchange = self.onchange;

        self.onchange = function(prop, a, b, c, d) {
            if (typeof(c) !== 'undefined') {
                // Version 4
                if (c !== d && typeof(onchange) === 'function') {
                    onchange.call(self, self, self.value);
                }
            } else {
                // Version 5
                if (a !== b && typeof(onchange) === 'function') {
                    onchange.call(self, self, self.value);
                }
            }
        }

        self.onload = function() {
            if (self.width) {
                self.el.style.width = self.width;
            }
        }

        return `<label class="lm-switch" position="{{self.position}}" data-color="{{self.color}}">
            <input type="checkbox" name="{{self.name}}" disabled="{{self.disabled}}" :bind="self.value" /> <span>{{self.text}}</span>
        </label>`
    }

    // Create LemonadeJS references
    lemonade.setComponents({ Switch: Switch });
    // Create web-component
    lemonade.createWebComponent('switch', Switch);

    return function (root, options) {
        if (typeof (root) === 'object') {
            lemonade.render(Switch, root, options)
            return options;
        } else {
            return Switch.call(this, root);
        }
    }

})));