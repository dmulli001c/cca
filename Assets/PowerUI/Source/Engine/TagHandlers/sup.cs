//--------------------------------------
//               PowerUI
//
//        For documentation or 
//    if you have any issues, visit
//        powerUI.kulestar.com
//
//    Copyright � 2013 Kulestar Ltd
//          www.kulestar.com
//--------------------------------------

namespace PowerUI{
	
	/// <summary>
	/// Handles the standard sup(erscript) element.
	/// </summary>
	
	public class SupTag:HtmlTagHandler{
		
		public override string[] GetTags(){
			return new string[]{"sup"};
		}
		
		public override Wrench.TagHandler GetInstance(){
			return new SupTag();
		}
		
	}
	
}