# Yort.Humm.InStore
An unofficial .Net client library for the Humm payments POS API (https://docs.shophumm.com.au/pos/getting-started/)

# WORK IN PROGRESS

# Status
![Yort.Humm.Instore.Build](https://github.com/Yortw/Yort.Humm.InStore/workflows/Yort.Humm.Instore.Build/badge.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) [![Coverage Status](https://coveralls.io/repos/github/Yortw/Yort.Humm.InStore/badge.svg?branch=master)](https://coveralls.io/github/Yortw/Yort.Humm.InStore?branch=master)

# Documentation

Make sure you read the [Humm API documentation](https://docs.shophumm.com.au/pos/getting-started/). This library is a light-weight, idiomatic .Net library, around the Humm API. 
It mostly eliminates busy/technical work for you, such as implementing the digital signature algorithm, checking signatures on responses and providing 
request/response models for the endpoints. Understanding the Humm API will help you understand what is possible and how to use this library.

That said, this library does include a few (optional) convenience features, such as auto-retrying a ProcessAuthorisation response with a pending status, and a class for selecting 
the relevant API end points based on logical selection criteria (country, api version, api enviroment). The main client object also supports an interface, as well as HttpClient injection, providing two different ways of mocking/stubbing out the standard behaviour if you need to write automated tests against code using this library.

For getting started, samples and API reference [see the docs](https://yortw.github.io/Yort.Humm.InStore/docs/index.html)
