// 
// Log.cs
//  
// Author:
//       Anirudh Sanjeev <anirudh@anirudhsanjeev.org>
// 
// Copyright (c) 2010 Anirudh Sanjeev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace ScreenLapse
{
	public static class Log
	{
		/// <summary>
		/// Sends the input to stdout/log file/etc
		/// </summary>
		/// <param name="input">
		/// A <see cref="System.String"/>
		/// </param>
		private static void Emit(ConsoleColor color, string level, string input)
		{
			Console.WriteLine (String.Format("[{0}][THREAD:{1}] {2}", level, System.Threading.Thread.CurrentThread.ManagedThreadId ,input));
		}
		
		public static void Debug(string input)
		{
			Emit(ConsoleColor.White, "DEBUG", input);
		}

		public static void Info(string input)
		{
			Emit(ConsoleColor.Green, "INFO", input);
		}

		public static void Warn(string input)
		{
			Emit(ConsoleColor.DarkRed, "WARN", input);
		}
		public static void Error(string input)
		{
			Emit(ConsoleColor.Red, "ERROR", input);
		}

	}
}

