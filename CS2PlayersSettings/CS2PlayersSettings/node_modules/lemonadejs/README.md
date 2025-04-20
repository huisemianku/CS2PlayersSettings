# LemonadeJS v4: Reactive micro library

## Create amazing web-based interfaces with LemonadeJS

![Micro Library](https://lemonadejs.com/templates/default/img/lemonadejs-home.jpg)

LemonadeJS is a super lightweight reactive vanilla javascript micro-library (7 KBytes). It helps to integrate the JavaScript (controllers) and the HTML (view). It supports two-way data binding and integrates natively with jSuites to help to create amazing interfaces quicker.<br><br>

It would help you deliver reusable components and does not require transpiler, babel, or hundreds of other dependencies. It works just fine in any javascript dev environment. LemonadeJS has a quick learning curve, keeps coding fun, and is very close to native JS.

- Make rich and user-friendly web interfaces and applications
- Handle complicated data inputs with ease and convenience
- Improve the software user experience
- Create rich CRUDS and beautiful UI
- Highly flexible and customizable
- Lightweight and simple to use


## Installation

### NPM package

```bash
% npm install lemonadejs
```

### Using from CDN

```html
<script src="https://cdn.jsdelivr.net/npm/lemonadejs/dist/lemonade.min.js"></script>
```

### Create a LemonadeJS sample app

```
% npx @lemonadejs/create myApp<br>
% cd myApp<br>
% npm run start<br>
```

### Running tests

```
% npm run test
```

## Examples

### Webpack

Build modern applications with lemonadeJS and node.

[See this example on codesandbox](https://codesandbox.io/s/reactive-micro-library-ny99bk)

```javascript
import lemonade from "lemonadejs";
import Hello from "./Hello";

export default function App() {
  let self = this;
  self.count = 1;
  return `<div>
        <div><Hello /></div>
        <p>You clicked {{self.count}} times</p>
        <button onclick="self.count++;">Click me</button>
  </div>`;
}
```

### Browser

Simplicity to run in the browser without dependencies, servers, transpiler.<br>

```html
<html>
<body>
<div id="root"></div>
<script src="https://cdn.jsdelivr.net/npm/lemonadejs/dist/lemonade.min.js"></script>
<script>
function Hello() {
    let self = this;
    return `<h1>{{self.title}}</h1>`;
}

function App() {
    let self = this;
    self.count = 1;
    return `<>
      <Hello title="your title" />
      <p>You clicked {{self.count}} times</p>
      <button onclick="self.count++;">Click me</button>
    </>`;
}
lemonade.render(App, document.getElementById('root'));
</script>
</body>
</html>
```

### Creating a table from an array of objects

```javascript
import lemonade from "lemonadejs";

export default function Component() {
    let self = this;

    self.rows = [
        { title:'Google', description: 'The alpha search engine...' },
        { title:'Bing', description: 'The microsoft search engine...' },
        { title:'Duckduckgo', description: 'Privacy in the first place...' },
    ];

    // Custom components such as List should always be unique inside a real tag.
    return `<table cellpadding="6">
        <thead><tr><th>Title</th><th>Description</th></th></thead>
        <tbody @loop="self.rows">
            <tr><td>{{self.title}}</td><td>{{self.description}}</td></tr>
        </tbody>
    </table>`;
}
```

### The event object

```html
<html>
<body>
<div id='root'></div>
<script src="https://cdn.jsdelivr.net/npm/lemonadejs/dist/lemonade.min.js"></script>
<script>
function Component() {
    // Create the self object
    let self = this;
    self.test = function(e) {
        console.log(e);
        e.preventDefault();
    }
    // The property call is added to the observable list when added to the DOM
    return `<input type="button" value="Click test" onclick="self.test(e);"/>`;
}

// Render the LemonadeJS element into the DOM
lemonade.render(Component, document.getElementById('root'));
</script>
</body>
</html>
```

### Enable/disable HTML elements

```html
<html>
<body>
<div id='root'></div>
<script src="https://cdn.jsdelivr.net/npm/lemonadejs/dist/lemonade.min.js"></script>
<script>
function App() {
    let self = this;
    self.disabled = false;
    return `<>
      <button onclick="self.disabled = !self.disabled">Toggle</button>
      <input type="text" disabled="{{self.disabled}}" />
    </>`;
}
lemonade.render(App, document.getElementById('root'));
</script>
</body>
</html>
```

### Reactive Web Components

```html
<hello-element title="Hello world" />
```

```javascript
class HelloElement extends HTMLElement {
    constructor() {
        super();
    }
 
    render() {
        let self = this;
        return `<>
            <h1>{{self.title}}</h1>
            <input type="button" value="setTitle()"
                onclick="self.title = 'Test'" />
        </>`;
    }
 
    connectedCallback() {
        if (! this.el) {
            lemonade.render(this.render, this, this);
        }
    }
}
 
window.customElements.define('hello-element', HelloElement);
```


## License

This software is free to use, and it is distributed under the MIT license.

## Learning LemonadeJS

### Documentation

* [Getting started](https://lemonadejs.com/docs/getting-started)
* [Attributes](https://lemonadejs.com/docs/attributes)
* [Two-way binding](https://lemonadejs.com/docs/two-way-binding)
* [Arrays](https://lemonadejs.com/docs/arrays)
* [Methods](https://lemonadejs.com/docs/methods)
* [Events](https://lemonadejs.com/docs/events)
* [Components](https://lemonadejs.com/docs/components)
* [Classes](https://lemonadejs.com/docs/classes)
* [Web components](https://lemonadejs.com/docs/web-components)
* [Quick reference](https://lemonadejs.com/docs/quick-reference)
* [Debugging](https://lemonadejs.com/docs/debugging)
* [Contributing](https://lemonadejs.com/docs/contributions)


### Utilities

* [Awesome](https://lemonadejs.com/docs/awesome)
* [Sugar](https://lemonadejs.com/docs/sugar)
* [Testing](https://lemonadejs.com/docs/tests)
* [Plugins](https://lemonadejs.com/docs/plugins)
* [Module (ESM)](https://lemonadejs.com/docs/module)


### Libraries

* [JavaScript List](https://lemonadejs.com/docs/plugins/list)
* [JavaScript Rating](https://lemonadejs.com/docs/plugins/rating)
* [JavaScript Router](https://lemonadejs.com/docs/plugins/router)
* [JavaScript Signature](https://lemonadejs.com/docs/plugins/signature)
* [JavaScript Data grid](https://lemonadejs.com/docs/plugins/data-grid)
* [JavaScript Image Cropper](https://lemonadejs.com/docs/plugins/image-cropper)
* [JavaScript Modal](https://lemonadejs.com/docs/plugins/modal)
* [JavaScript Tabs](https://lemonadejs.com/docs/plugins/tabs)
* [JavaScript Calendar](https://lemonadejs.com/docs/plugins/calendar)
* [JavaScript Color picker](https://lemonadejs.com/docs/plugins/color-picker)


### Examples

* [Lamp](https://lemonadejs.com/docs/examples/lamp">)
* [Counter](https://lemonadejs.com/docs/examples/counter">)
* [Color generator](https://lemonadejs.com/docs/examples/color-generator">)
* [Value persistence](https://lemonadejs.com/docs/examples/value-persistence">)
* [DIV onresize](https://lemonadejs.com/docs/examples/div-onresize">)
* [Star rating](https://lemonadejs.com/docs/examples/rating">)
* [Table](https://lemonadejs.com/docs/examples/table">)
* [Disable elements](https://lemonadejs.com/docs/examples/enable-disable-elements">)
* [Color picker](https://lemonadejs.com/docs/examples/color-picker">)
* [Hangman game](https://lemonadejs.com/docs/examples/hangman">)
* [Tic tac toe](https://lemonadejs.com/docs/examples/tic-tac-toe">)


## Other tools

- [JavaScript Components](https://jsuites.net/)
- [JavaScript Data Grid](https://jspreadsheet.com/)
- [Free JavaScript Data Grid](https://bossanova.uk/jspreadsheet/)
