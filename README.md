# Snapper Coding Challenge

## Implementation

Approximate search within matrix based on pattern that is also matrix.

Go through big matrix and to match all occurrences of smaller matrix.

Search within matrix implemented in iterative way.
- Iterate over bigger matrix trying to find closest occurrence of smaller matrix
- Percentage match check using matchingElements / totalElements >= threshold
- Average per row match (avgMatchPercent >= (1 - tolerance)
	
##  NFM Non-negative matrix factorization pattern recognition
`out of scope`

Custom implementation could be away to hard.
There is a library NMF.Transform

```
	MathNet.Numerics.LinearAlgebra;
	MathNet.Numerics.LinearAlgebra.Double;
	NMF.Transform;
```