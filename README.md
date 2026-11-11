# Repeat Reading

Build a measurement uncertainty budget for each quantity with units and explicit assumptions.

## Try it

From this folder with the .NET 10 SDK installed:

```sh
dotnet run -- budget study.tsv
```

Each row gives quantity, unit, reading, instrument resolution step and optional additional standard uncertainty (write 0 for none). Within a group use the same instrument terms. Combines independent standard uncertainties in quadrature. Resolution error is uniform over ±half a step. A singleton reports known components with total undefined.

The included study.tsv uses tab-separated fields. Comment lines start with `#`.

The original exercise remains available with `dotnet run -- example.tsv`.

## When to use it

A lab student needs a defensible uncertainty result from repeats and a measuring instrument of finite resolution.

Example result:

```text
Length [mm]: n=2, mean 11, resolution u=0.0288675, other independent u=0.02
  repeatability u(mean)=1; combined standard uncertainty 1.00062 mm
Mass [g]: n=1, mean 5, resolution u=0.288675, other independent u=0
  repeatability unavailable; combined standard uncertainty undefined
```

Related course or field workflow: [CSULB physics lab on uncertainty](https://home.csulb.edu/~tgredig/phys152lab/uncertainties.html).
