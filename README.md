# Spell

Spell provide phrases which is only known you.


## Get Started

### Random-phase generation

```csharp
// spell is byte array.
var spell = HocusPocus.Generate('your phrase which is external factor');
```


### Spell calling

Spell generate fixed-phrase of corresponding salt and seed.

```csharp
// spell is byte array.
var spell = Spell.Create('your salt', 'your seed');
```


### To String
Spell can convert to printable string.

```csharp
// e.g. to ASCII string
var printablePhrase = spell.With(new Asciinize());
```

## License

Whoami is licensed under the [MIT license](LICENSE).
