# YoctoMVVM Portable

## General

YoctoMVVM is a __minimalistic__ framework aimed at allowing an easy implementation of the MVVM pattern.

It is composed of the following components:

* Bindable class for observable properties. View models and other types should inherit this class.
* Commands to allow easy data binding. An async variant also exists.
* Small IoC container for type registering and resolving.
* Courier - Implements a mediator to allow part of the app to communicate without forcing dependencies.
* Platform Abstraction - Both a PCL and platform specific approaches exist.

## Portable
It is recommended to use the portable solution if you can use a PCL. Otherwise, you can use the platform specific implementations in the top-level folders.

## Theme
The YoctoMVVM framework evolved on a part by part basis, as the need arose. It is by far the smallest framework of the bunch, because it only contains the components necessary for MVVM implementation.

## Name
Yocto is the smallest named number prefix, which represents 10<sup>-24</sup>.

## License
YoctoMvvm is released under the MIT license.

Copyright &copy; 2013 Erez A. Korn

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

