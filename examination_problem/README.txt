## Self assessment: 9 or 10, you decide :D
My integrator works well, not only with basic examples.
I implemented 2 more (!) methods that I think are both good for comparisons.
The comparisons are interesting and pretty much exactly what you would expect from each.


## Explanation of my work

As requested, I made a 2D adaptive integrator, using the Simpson 3/8 rule. 

I tested this on some simple examples with good results.

What I think is a weakness of this method, is that it adapts in x and y seperately and might not catch diagonal structures.

Therefore I implemented 2 more methods that are grid based:
- midpoint method: this basically brute forces, meaning it makes the grid finer until accuracy is met
- quadtree method (short quad): this is, like simpson method, working recursively but on a grid
Both of these needed a wrapper/masked function to be able to handle curved boundaries.

To be able to compare them, I made my class able to output the amount of function calls for each method.

Short explanations of each method:
## Simpson
- Integrates over `y` adaptively for each `x`
- Then integrates those results over `x`, also adaptively
- Supports curved domains via variable limits (`d(x)` to `u(x)`)

---

## Midpoint
- Divides the domain into a uniform `n × n` grid
- Evaluates function at the midpoint of each cell
- Doubles resolution until the result converges

---

## Quad
- Recursively subdivides the domain into 4 quadrants (quadtree)
- Estimates integral using the midpoint of each tile
- Refines only where local error is too large
- Fully adaptive in both `x` and `y`
