# msgpack.spec

Reference implementation of https://github.com/msgpack/msgpack/blob/master/spec.md

## Usage

When you're sure what you're doing, use `WriteXXXX` methods, they are faster, but can throw exceptions.
`TryXXXX` methods are more convinient if you're not sure about anything.
