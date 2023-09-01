# Grid-Mesh

## Overview
This library was developed as a tool for obtaining specific percentages of boolean calculations when working with mesh. The idea involves taking a model's mesh, dividing it based on a specified resolution, and determining whether the center point of each grid is inside the model's mesh. If it is, a collision sphere of a specified size is placed.

The library provides both the original conceptual code and the optimized version using Job System and Burst, which resulted in an efficiency improvement of over 400 times.

As for adapting it for other functionalities, you can modify it according to your requirements.
## Implement
`Test Case`
```c++
Verices:1477
Indices:8850
Cell Size: (0.2f,0.2f,0.2f)
```
### Source Code
```c++
Generate time : 00:00:18.3074090
```
### Accelerated using Job System and Burst
427 times the efficiency of the source code
```c++
Generate time : 00:00:00.0439782
```