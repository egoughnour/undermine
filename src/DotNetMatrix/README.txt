

The code in this project section has been taken from
http://www.codeproject.com/KB/recipes/psdotnetmatrix.aspx

which in turn takes it from JAMA: http://math.nist.gov/javanumerics/jama/


The code has tested for a limited set of operations and may have further bugs. 

Additionally, the code should not be used for programs requiring too much matrix computation. See the performance below.


PERFORMANCE: The code uses Jagged arrays. For heavy matrix operations, it is preferred that you use Rectangular arrays in C#. 
Please see the following article for that
http://msdn2.microsoft.com/hi-in/magazine/cc163995(en-us).aspx


