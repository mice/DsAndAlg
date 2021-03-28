using System;

public static class Karastsub
{
    public static int CountOne(uint n)
    {
        int res = 0;
        for (; n != 0; n &= n - 1)
            ++res;
        return res;
    }

    // FOLLOWING TWO FUNCTIONS ARE COPIED FROM http://goo.gl/q0OhZ 
    // Helper method: given two unequal sized bit strings, converts them to 
    // same length by adding leading 0s in the smaller string. Returns the 
    // the new length 
    public static int makeEqualLength(ref string  str1,ref string str2) 
    { 
        int len1 = str1.Length;
        int len2 = str2.Length;
        if (len1 < len2) 
        { 
            for (int i = 0 ; i < len2 - len1 ; i++) 
                str1 = '0' + str1; 
            return len2; 
        } 
        else if (len1 > len2) 
        { 
            for (int i = 0 ; i < len1 - len2 ; i++) 
                str2 = '0' + str2; 
        } 
        return len1; // If len1 >= len2 
    } 
    
    // The main function that adds two bit sequences and returns the addition 
    public static string addBitStrings(ref string first,ref string second ) 
    { 
        string result = "";  // To store the sum bits 
    
        // make the lengths same before adding 
        int length = makeEqualLength(ref first,ref second); 
        int carry = 0;  // Initialize carry 
    
        // Add all bits one by one 
        for (int i = length-1 ; i >= 0 ; i--) 
        { 
            int firstBit = first[i] - '0'; 
            int secondBit = second[i] - '0'; 
    
            // boolean expression for sum of 3 bits 
            int sum = (firstBit ^ secondBit ^ carry)+'0'; 
    
            result = (char)sum + result; 
    
            // boolean expression for 3-bit addition 
            carry = (firstBit&secondBit) | (secondBit&carry) | (firstBit&carry); 
        } 
    
        // if overflow, then add a leading 1 
        if (carry!=0)  result = '1' + result; 
    
        return result; 
    } 
    
    // A utility function to multiply single bits of strings a and b 
    public static int multiplyiSingleBit(string a, string b) 
    {  return (a[0] - '0')*(b[0] - '0');  } 
    
     //1. 计算n位
    //2. 计算是否为2的m次方,新思路为补到2的M次方
    //3. 递归计算
    // The main function that multiplies two bit strings X and Y and returns 
    // result as long integer 
    public static long multiply(string X, string Y) 
    { 
        // Find the maximum of lengths of x and Y and make length 
        // of smaller string same as that of larger string 
        int n = makeEqualLength(ref X,ref Y); 
    
        // Base cases 
        if (n == 0) return 0; 
        if (n == 1) return multiplyiSingleBit(X, Y); 
    
        int fh = n/2;   // First half of string, floor(n/2) 
        int sh = (n-fh); // Second half of string, ceil(n/2) 
    
        // Find the first half and second half of first string. 
        // Refer http://goo.gl/lLmgn for substr method 
        string Xl = X.Substring(0, fh); 
        string Xr = X.Substring(fh, sh); 
    
        // Find the first half and second half of second string 
        string Yl = Y.Substring(0, fh); 
        string Yr = Y.Substring(fh, sh); 
    
        // Recursively calculate the three products of inputs of size n/2 
        long P1 = multiply(Xl,Yl); 
        long P2 = multiply(Xr,Yr); 
        long P3 = multiply(addBitStrings(ref Xl,ref Xr), addBitStrings(ref Yl,ref Yr)); 
    
        // Combine the three products to get the final result. 
        return P1*(1<<(2*sh)) + (P3 - P1 - P2)*(1<<sh) + P2; 
    }
}