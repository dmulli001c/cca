/// <copyright>
/// Copyright (c)2015, TopCoder, Inc. All rights reserved
/// </copyright>
/// <version>1.0.0</version>
/// <author>engkfke</author> 


interface IRequest
{
    /// <summary>
    /// Processes this instance.
    /// </summary>
    /// <returns></returns>
    IResponse Process();
}

interface IResponse
{
}