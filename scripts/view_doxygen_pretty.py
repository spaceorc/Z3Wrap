#!/usr/bin/env python3
"""
Pretty-print a function's Doxygen XML documentation in a readable format.
Usage: python view_doxygen_pretty.py <function_name>
"""

import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def get_text(elem):
    """Extract all text from an element and its children."""
    if elem is None:
        return ""
    text = elem.text or ""
    for child in elem:
        text += get_text(child)
        text += child.tail or ""
    return text.strip()


def view_function_pretty(xml_path: Path, func_name: str):
    """View a function's documentation from Doxygen XML in a pretty format."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this function
    for memberdef in root.findall(".//memberdef[@kind='function']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == func_name:
            print(f"\n{'='*80}")
            print(f"Function: {func_name}")
            print('='*80)

            # Type and signature
            type_elem = memberdef.find("type")
            argsstring = memberdef.find("argsstring")
            print(f"\nSignature: {get_text(type_elem)} {func_name}{get_text(argsstring)}")

            # Parameters
            print("\nParameters:")
            for param in memberdef.findall("param"):
                param_type = get_text(param.find("type"))
                param_name = get_text(param.find("declname"))
                array = param.find("array")
                if array is not None:
                    param_type += "[]"
                print(f"  - {param_name}: {param_type}")

            # Brief description
            brief = memberdef.find("briefdescription")
            if brief is not None:
                brief_text = get_text(brief)
                if brief_text:
                    print(f"\nBrief:")
                    print(f"  {brief_text}")

            # Detailed description
            detailed = memberdef.find("detaileddescription")
            if detailed is not None:
                # Body paragraphs (not in special sections)
                print("\nDetailed Description:")
                for para in detailed.findall("para"):
                    # Skip paragraphs that only contain parameterlist or simplesect
                    if para.text and para.text.strip():
                        print(f"  {para.text.strip()}")

                # Parameter documentation
                for paramlist in detailed.findall(".//parameterlist[@kind='param']"):
                    print("\nParameter Documentation:")
                    for item in paramlist.findall("parameteritem"):
                        param_name = get_text(item.find(".//parametername"))
                        param_desc = get_text(item.find(".//parameterdescription"))
                        print(f"  @param {param_name}: {param_desc}")

                # Preconditions
                for pre in detailed.findall(".//simplesect[@kind='pre']"):
                    print(f"\nPrecondition:")
                    print(f"  {get_text(pre)}")

                # Remarks
                for remark in detailed.findall(".//simplesect[@kind='remark']"):
                    print(f"\nRemark:")
                    print(f"  {get_text(remark)}")

                # See also
                for see in detailed.findall(".//simplesect[@kind='see']"):
                    refs = see.findall(".//ref")
                    if refs:
                        print("\nSee Also:")
                        for ref in refs:
                            print(f"  - {get_text(ref)}")

            # Location
            location = memberdef.find("location")
            if location is not None:
                file = location.get("file", "")
                line = location.get("line", "")
                print(f"\nLocation: {file}:{line}")

            print('='*80)
            return

    print(f"Function '{func_name}' not found")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python view_doxygen_pretty.py <function_name>")
        print("Example: python view_doxygen_pretty.py Z3_mk_tuple_sort")
        sys.exit(1)

    func_name = sys.argv[1]
    project_root = Path(__file__).parent.parent
    xml_path = project_root / ".cache" / "doxygen" / "xml" / "group__capi.xml"

    view_function_pretty(xml_path, func_name)
