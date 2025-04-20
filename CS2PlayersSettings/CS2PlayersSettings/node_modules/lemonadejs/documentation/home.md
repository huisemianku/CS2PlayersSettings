title: LemonadeJS - Agnostic Micro Reactive JavaScript Library
keywords: LemonadeJS, two-way binding, frontend, javascript library, javascript plugin, javascript, reactive, react
description: LemonadeJS is a lightweight vanilla javascript reactive micro library (7Kb) that helps the integration between the JavaScript and the HTML.
canonical: https://lemonadejs.com

<div class="homepage">

<div class="row">
    <div class="column f1">
        <div class="limit-75">
            <h1>Agnostic Micro Reactive JavaScript Library</h1>
            <p class="small">This library is about 4 KBytes compressed.</p>
            <p>LemonadeJS is a compact, reactive agnostic JavaScript micro-library, weighing in at just 4KB when compressed. It facilitates two-way data binding and is designed to assist in creating modern, reusable components. The library is dependency-free and does not require transpiling, streamlining the development process.</p>
            <p>No <b>dependencies</b> or <b>transpile</b> is required.</p>
            <br>
            <button type="button" class="button main"><a href="/docs/getting-started">Getting started</a></button>
        </div>
    </div><div class="column f1 p20">
        <img src='img/lemonadejs-home.jpg' class='w100' alt="LemonadeJS reactive library" style="max-width: 600px;">
    </div>
</div>

<br><br><br>

<div class="row">
<div class="column f1 mr2">
    <h2>Modern interfaces</h2>
    <p>Build applications that meet Content Security Policy (CSP) standards using Webpack, or run them directly in standard web browsers.</p>
    <div class='example'><lm-demo></lm-demo></div>
    <div class="link">
        <a href='https://codesandbox.io/s/reactive-micro-library-ny99bk' target="_blank">See this example on codesandbox</a>
    </div>

</div><div class="column f1">
<pre>
<code>
import lemonade from "lemonadejs";
// Component Hello
function Hello() {
    let self = this;
    return `&#60;div>You clicked {{self.number}} times&#60;/div>`;
}
// Create the component
export default function App() {
    let self = this;
    self.count = 1;
    self.click = () => {
        self.count++;
    }
    return `&#60;>
        &#60;Hello :number="self.count" />
        &#60;input type="button" onclick="self.click" value="Click me" />
    &#60;/>`;
}
// Register the dependencies to be used across the application
lemonade.setComponents({ Hello });
</code>
</pre>
</div>
</div>

<br><br><br>

<div class="row">
<div class="column f1 mr2">
    <h2>Simplicity</h2>
    <p>Code directly in your browser with no need for dependencies or transpiling.</p>
    <h3>Flexibility</h3>
    <ul>
        <li>Easily integrate LemonadeJS into any existing application.</li>
        <li>Apply LemonadeJS to particular areas within existing applications.</li>
        <li>Implement two-way data binding for specific DOM elements.</li>
    </ul>
    <h3>Properties</h3>
    <ul>
        <li>Two-way data binding for dynamic applications.</li>
        <li>Ultra-lightweight footprint (only 4KB when compressed).</li>
        <li>Reactive interfaces that respond to data changes in real time.</li>
        <li>Optimized for a superior user experience.</li>
        <li>Compatible with Webpack or can be used directly in web browsers.</li>
        <li>Zero dependencies are required.</li>
    </ul>
    <br>
    <div class="link">
        <a href='https://jsfiddle.net/spreadsheet/rgx59suc/' target="_blank">See this example on jsfiddle</a>
    </div>
</div><div class="column f1">
<pre>
<code>
&#60;html>
&#60;script src="https://lemonadejs.com/lemonade.js">&#60;/script>
&#60;div id="root">&#60;/div>
&#60;script>
// Component Hello
function Hello() {
    let self = this;
    return `&#60;div>You clicked {{self.number}} times&#60;/div>`;
}
// Create the component
function App() {
    let self = this;
    self.count = 1;
    self.click = () => {
        self.count++;
    }
    return `&#60;>
        &#60;Hello :number="self.count" />
        &#60;input type="button" onclick="self.click" value="Click me" />
    &#60;/>`;
}
// Register the dependencies to be used across the application
lemonade.setComponents({ Hello });
// Render the component
lemonade.render(App, document.getElementById('root'));
&#60;/script>
&#60;/html>
</code>
</pre>
</div>
</div>

<br><br><br>

<div>
<h2>How it Works</h2>
<h3>Template Literals</h3>
LemonadeJS utilizes JavaScript template literals as a foundational mechanism. This approach helps bind the data with the `self` declarations,
track changes, and synchronize the data and the view. A key advantage of LemonadeJS is its ability to make precise, node-specific updates
in the DOM, eliminating the need for a complete component refresh.
<br><br>
LemonadeJS's defining feature is its capacity to act as an intermediate reactive layer. This flexibility allows the creation from
full native LemonadeJS applications or simple facilitates the integration of LemonadeJS
components with popular frameworks such as React, Vue, and Angular. Additionally, its ability to target specific DOM sections makes
it an ideal choice for augmenting various parts of an application without being restricted to a particular framework or technology.

<h4>Library, Not a Framework</h4>
LemonadeJS distinguishes itself by being a micro-library rather than a full-fledged framework. This distinction is crucial as it implies
that LemonadeJS's packages and components are incredibly lightweight yet highly effective. The library provides total reactivity,
supports two-way data binding, and includes other advanced features. This compact yet robust design makes LemonadeJS a versatile
tool for developers looking to add reactivity to their projects without the overhead of a larger framework.

<h3>Special Attributes</h3>
LemonadeJS introduces a set of unique attributes specifically designed to facilitate the creation of reactive components.
These attributes enable two-way data binding, reactive behaviours, and other dynamic features essential for modern web
applications. Including these attributes underscores the library's commitment to providing developers with tools to
build highly responsive and interactive user interfaces with much less code than is typically required.

<h3>Official components</h3>
In addition to its core features, LemonadeJS offers a collection of official JavaScript reactive components. These
components address everyday needs in web application development and are designed to be compatible with pure
JavaScript, LemonadeJS or other significant frameworks like React, AngularJS  or VueJS projects. This broad
compatibility ensures developers can leverage LemonadeJS components in various contexts, enhancing the library's
utility and applicability in the web development ecosystem.

Here is a list of a few featured components:<br><br>

<ul>
    <li><a href="/docs/plugins/data-grid">JavaScript Data grid</a></li>
    <li><a href="/docs/plugins/modal">JavaScript Modal</a></li>
    <li><a href="/docs/plugins/calendar">JavaScript Calendar</a></li>
    <li><a href="/docs/plugins/dropdown">JavaScript Dropdown</a></li>
    <li><a href="/docs/plugins/color-picker">JavaScript Color picker</a></li>
    <li><a href="/docs/plugins/timeline">JavaScript Timeline</a></li>
    <li><a href="/docs/plugins/context-menu">JavaScript Context Menu</a></li>
    <li><a href="/docs/plugins/tabs">JavaScript Tabs</a></li>
    <li><a href="/docs/plugins/image-cropper" title="Photo cropper and filters component">JavaScript Image cropper</a></li>
</ul>

</div>

<br><br><br>

<div>
<h2>Installation</h2>

<pre>
<code>% npm install lemonadejs</code>
</pre>

Or download from our <a href='http://github.com/lemonadejs/lemonadejs' class='link'>Github Official</a>

<h2>Utilities</h2>

<h3>Unit Tests</h3>

The LemonadeJS Tester simplifies the process of creating unit tests for your components, ensuring your code is robust and reliable.<br>

<h3>The Pico Library</h3>

This library provides a collection of essential JavaScript components. Each component in the library is dependency-free and has a size limit of just 2 KBytes, making it both lightweight and efficient.<br>

<h3>Sugar (Super Global Artifacts)</h3>

LemonadeJS Sugar is a shared global container that streamlines communication between different components, enabling seamless integration and improved collaboration within your projects.<br>

<h2>History</h2>

<h3>LemonadeJS v4.2.0</h3>
<ul>
    <li>New method to create web components.</li>
</ul>

<h3>LemonadeJS v4.1.0</h3>
<ul>
    <li>Re-setting the same value to a property won't dispatch changes.</li>
</ul>

<h3>LemonadeJS v4.0.0</h3>
<ul>
    <li>Tracking complex nested objects with arrays</li>
    <li>CSP compliance syntax</li>
    <li>Literals template binding references to DOM attributes</li>
</ul>

<h3>LemonadeJS v3.5.0</h3>
<ul>
    <li>Any attribute can receive prefix : to receive references</li>
</ul>

<h3>LemonadeJS v3.0.0</h3>
<ul>
    <li>Partial properties value update, for example: class="something {{self.somethingElse}}"</li>
    <li>Better deal with tag and textNodes {{self.something}} as a text node along a tag</li>
    <li>Sub-level property tracking. &#60;h1>{{self.level.value}}&#60;/h1></li>
    <li>Parent tracking and two-way binding. &#60;h1>{{self.parent.title}}&#60;/h1></li>
    <li>Unit tests</li>
</ul>

<h3>LemonadeJS v2.8.9</h3>
<ul>
    <li>self.refresh to rebuild the component keeping the self state.</li>
    <li>Special attribute :src + default property</li>
    <li>Support closing tags for web-components</li>
    <li>Self attributes from classes</li>
</ul>

<h3>LemonadeJS v2.7.2</h3>
<ul>
    <li>lm-ready, lm-loop, lm-ref, lm-bind alternative for :ready, :loop, :ref, :bind</li>
    <li>Components are now case-insensitive</li>
    <li>A new simple :bind for custom components</li>
</ul>

<h3>LemonadeJS v2.4.2</h3>
<ul>
    <li>lemonade.dictionary, lemonade.translate. (text translation support)</li>
    <li>run evaluation method</li>
</ul>

<h3>LemonadeJS v2.2.4</h3>
<ul>
    <li>The event object (e) is available on the template, example. onclick="self.test(e)"</li>
</ul>

<h3>LemonadeJS v2.2.0</h3>
<ul>
    <li>Support for tables</li>
</ul>

<h3>LemonadeJS v2.1.13</h3>
<ul>
    <li>:loop on native HTML tags.</li>
    <li>Precedence for val() on setAttributes</li>
    <li>self.parent as reserved property to get the self of the a caller inside custom elements</li>
</ul>

<h3>LemonadeJS v2.1.7</h3>
<ul>
    <li>Global queue</li>
    <li>Pico Library</li>
    <li>Sugar Common Container</li>
    <li><code>self.el</code> as a reserved property which returns the root element</li>
    <li>Real time templates for custom elements.</li>
    <li>Extensions and template to the children on custom components</li>
</ul>

<h3>LemonadeJS v2.0.6</h3>
<ul>
    <li>:loop property</li>
    <li>Better integration with components</li>
    <li>Custom component templating</li>
</ul>

<h3>LemonadeJS v1.0.0</h3>
<ul>
    <li>:bind, :ready, :ref magic properties.</li>
</ul>

<h2>Copyright and license</h2>

LemonadeJS is released under the MIT license. Contact <a href="mailto:support@jspreadsheet.com">LemonadeJS</a></p>

<h2>About LemonadeJS</h2>

LemonadeJS is a unique, micro-reactive JavaScript library designed to streamline web development integration of HTML, data, and JavaScript methods. This efficient and flexible solution enables developers to build user interfaces while staying as close to vanilla JavaScript as possible. With its intelligent concepts and powerful optimization, LemonadeJS provides a user-friendly learning curve, allowing developers to extend their work to the browser environment while creating modern, complex UIs and reusable components.
<br><br>
In its latest release, LemonadeJS introduces new concepts such as SUGAR (Super Global Artifacts), a shared container that simplifies communication between components. PICO, the official component library with zero dependencies, also weighs in at just 2 KBytes. This lightweight library enables seamless integration and efficient web development.

</div>
</div>