#region

using System;
using System.Text;

#endregion

namespace Hyldahl.Hashing.SpamSum
{
  public sealed class SpamSumSignature : IEquatable<SpamSumSignature>
  {
    /*****************************************************
       * FIELDS
       *****************************************************/

    /*****************************************************
       * CONSTRUCTOR
       *****************************************************/

    /// <summary>
    ///   Initializes a new instance of the <see cref="SpamSumSignature" /> class.
    /// </summary>
    /// <param name="signature">The signature.</param>
    public SpamSumSignature(string signature)
    {
      if (string.IsNullOrEmpty(signature))
        throw new ArgumentException("Signature string cannot be null or empty.", "signature");

      var idx1 = signature.IndexOf(':');
      var idx2 = signature.IndexOf(':', idx1 + 1);

      if (idx1 < 0)
        throw new ArgumentException("Signature is not valid.", "signature");

      if (idx2 < 0)
        throw new ArgumentException("Signature is not valid.", "signature");

      BlockSize = uint.Parse(signature.Substring(0, idx1));
      HashPart1 = Encoding.ASCII.GetBytes(signature.Substring(idx1 + 1, idx2 - idx1 - 1));
      HashPart2 = Encoding.ASCII.GetBytes(signature.Substring(idx2 + 1));
    }

    public SpamSumSignature(uint blockSize, byte[] hash1, byte[] hash2)
    {
      BlockSize = blockSize;
      HashPart1 = hash1;
      HashPart2 = hash2;
    }

    /*****************************************************
       * PROPERTIES
       *****************************************************/

    /// <summary>
    ///   Gets the size of the block.
    /// </summary>
    /// <value>The size of the block.</value>
    public uint BlockSize { get; }

    /// <summary>
    ///   Gets the first hash part.
    /// </summary>
    /// <value>The first hash part.</value>
    public byte[] HashPart1 { get; }

    /// <summary>
    ///   Gets the second hash part.
    /// </summary>
    /// <value>The second hash part.</value>
    public byte[] HashPart2 { get; }

    public bool Equals(SpamSumSignature other)
    {
      if (ReferenceEquals(this, other))
        return true;

      if (BlockSize != other.BlockSize)
        return false;

      if (HashPart1.Length != other.HashPart1.Length)
        return false;

      if (HashPart2.Length != other.HashPart2.Length)
        return false;

      for (var idx = 0; idx < HashPart1.Length; idx++)
        if (HashPart1[idx] != other.HashPart1[idx])
          return false;

      for (var idx = 0; idx < HashPart2.Length; idx++)
        if (HashPart2[idx] != other.HashPart2[idx])
          return false;

      return true;
    }

    /*****************************************************
       * METHODS
       *****************************************************/

    public override bool Equals(object obj)
    {
      if (!(obj is SpamSumSignature))
        return false;

      return Equals((SpamSumSignature) obj);
    }

    /*****************************************************
       * OPERATORS
       *****************************************************/

    /// <summary>
    ///   Performs an implicit conversion from <see cref="System.String" /> to <see cref="SpamSumSignature" />.
    /// </summary>
    /// <param name="signature">The signature.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator SpamSumSignature(string signature)
    {
      return new SpamSumSignature(signature);
    }

    public override string ToString()
    {
      var hashText1 = Encoding.ASCII.GetString(HashPart1);
      var hashText2 = Encoding.ASCII.GetString(HashPart2);
      return string.Format("{0}:{1}:{2}", BlockSize, hashText1, hashText2);
    }
  }
}