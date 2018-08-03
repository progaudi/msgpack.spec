# msgpack.spec

## Goal

Fast and with zero allocations implementation of [msgpack specification](https://github.com/msgpack/msgpack/blob/master/spec.md).

## Usage

When you're sure what you're doing, use `WriteXXXX` methods or `ReadXXXX`, they are faster, but can throw exceptions. `TryXXXX` methods are more convinient if you're not sure about anything.

## Speed

Numbers worth more than thousands of words. Below results of serialization of array of one hundred of large uints. As we can see, code in C#(`MsgPackSpecArray`) roughly twice as slow as native code. Can we go faster? Yes, we can, look to `PointerBigEndian` version: it's slower than C version only by 10%. But you usually don't want to right code like below. Let's hope that CLR team will improve Span<T> a lot more. And may be some more bounds checking will be eliminated.

```ini
BenchmarkDotNet=v0.11.0, OS=debian 9
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT


                Method |      Mean |    Error |   StdDev |    Median |        Q3 | Scaled | ScaledSD | Allocated |
---------------------- |----------:|---------:|---------:|----------:|----------:|-------:|---------:|----------:|
      MsgPackSpecArray | 170.75 ns | 3.391 ns | 6.286 ns | 169.92 ns | 175.26 ns |   1.75 |     0.07 |       0 B |
      PointerBigEndian | 103.19 ns | 1.181 ns | 1.047 ns | 102.86 ns | 103.49 ns |   1.06 |     0.02 |       0 B |
                CArray |  97.69 ns | 1.723 ns | 1.611 ns |  97.08 ns |  98.57 ns |   1.00 |     0.00 |       0 B |
              CppArray |  87.18 ns | 2.086 ns | 1.951 ns |  86.90 ns |  88.96 ns |   0.89 |     0.02 |       0 B |
```

Pointers in C# code:

```csharp
public unsafe void PointerBigEndian()
{
    fixed (byte* pointer = &_buffer[0])
    {
        pointer[0] = DataCodes.Array16;
        Unsafe.WriteUnaligned(ref pointer[1], length);
        for (var i = 0u; i < length; i++)
        {
            pointer[3 + 5 * i] = DataCodes.UInt32;
            Unsafe.WriteUnaligned(ref pointer[3 + 5 * i + 1], baseInt - i);
        }
    }
}
```
