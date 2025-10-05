#!/usr/bin/env python3
"""
View a specific function's Doxygen XML documentation.
Usage: python view_doxygen_function.py <function_name> [--brief]
"""

import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def extract_text_from_element(element, convert_refs_to_see_cref=False):
    """Extract all text content from an element and its children.

    Args:
        element: The XML element to extract text from
        convert_refs_to_see_cref: If True, convert <ref> elements to <see cref="..."/> tags
    """
    if element is None:
        return ""

    # Handle special cases where the element itself needs conversion
    if convert_refs_to_see_cref:
        if element.tag == 'ref':
            # This element is a reference
            ref_text = element.text if element.text else ""
            return f'<see cref="{ref_text}"/>'
        elif element.tag == 'computeroutput':
            # This element is inline code
            code_text = element.text if element.text else ""
            code_text = code_text.replace('‹ccode›', '')
            return f'<c>{code_text}</c>'
        elif element.tag == 'programlisting':
            # This element is a multi-line code block
            code_lines = []
            for codeline in element.findall('.//codeline'):
                # Extract text from highlight elements
                line_text = ''
                for elem in codeline.iter():
                    if elem.tag == 'sp':
                        line_text += ' '
                    elif elem.text:
                        line_text += elem.text
                    if elem.tail:
                        line_text += elem.tail
                if line_text.strip():
                    code_lines.append(line_text.strip())

            if code_lines:
                code_content = '\n'.join(code_lines)
                return f'<code>\n{code_content}\n</code>'
            return ''

    # Get text from this element and all children recursively
    text_parts = []
    if element.text:
        text_parts.append(element.text.strip())

    for child in element:
        # Check if this is a <ref> element (Doxygen reference to another function/type)
        if convert_refs_to_see_cref and child.tag == 'ref':
            ref_text = child.text if child.text else ""
            if ref_text:
                # Convert Z3 function name to C# method name
                # For now, just use the name as-is (we'll handle conversion later)
                text_parts.append(f'<see cref="{ref_text}"/>')
            # Add tail text if present
            if child.tail:
                text_parts.append(child.tail.strip())
        # Check if this is a <computeroutput> element (inline code)
        elif convert_refs_to_see_cref and child.tag == 'computeroutput':
            code_text = child.text if child.text else ""
            if code_text:
                # Remove our ‹ccode› marker if present
                code_text = code_text.replace('‹ccode›', '')
                # Convert to .NET inline code tag <c>
                text_parts.append(f'<c>{code_text}</c>')
            # Add tail text if present
            if child.tail:
                text_parts.append(child.tail.strip())
        else:
            child_text = extract_text_from_element(child, convert_refs_to_see_cref)
            if child_text:
                text_parts.append(child_text)
            if child.tail:
                text_parts.append(child.tail.strip())

    return ' '.join(text_parts)


def view_function_dotnet(xml_path: Path, func_name: str):
    """View a function's documentation as .NET XML comments."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this function
    for memberdef in root.findall(".//memberdef[@kind='function']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == func_name:
            print(f"/// <summary>")

            # Brief description
            brief_elem = memberdef.find("briefdescription")
            if brief_elem is not None:
                brief_text = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)
                if brief_text:
                    print(f"/// {brief_text}")

            print(f"/// </summary>")

            # Parameters
            detail_elem = memberdef.find("detaileddescription")
            if detail_elem is not None:
                param_list = detail_elem.find(".//parameterlist[@kind='param']")
                if param_list is not None:
                    for param_item in param_list.findall("parameteritem"):
                        param_name_elem = param_item.find(".//parametername")
                        param_name = param_name_elem.text if param_name_elem is not None else "?"
                        param_desc_elem = param_item.find(".//parameterdescription")
                        param_desc = extract_text_from_element(param_desc_elem, convert_refs_to_see_cref=True) if param_desc_elem is not None else ""
                        if param_desc:
                            print(f'/// <param name="{param_name}">{param_desc}</param>')

            # Returns
            if detail_elem is not None:
                return_sections = detail_elem.findall(".//simplesect[@kind='return']")
                if return_sections:
                    for return_section in return_sections:
                        return_text = extract_text_from_element(return_section, convert_refs_to_see_cref=True)
                        if return_text:
                            print(f"/// <returns>{return_text}</returns>")

            # Remarks (detail + preconditions + postconditions + warnings + remarks)
            remarks_parts = []

            # Detail paragraphs
            if detail_elem is not None:
                for para in detail_elem.findall("para"):
                    has_paramlist = para.find("parameterlist") is not None
                    has_simplesect = para.find("simplesect") is not None

                    if not has_paramlist and not has_simplesect:
                        para_text = extract_text_from_element(para, convert_refs_to_see_cref=True)
                        # Skip def_API lines
                        if para_text and not para_text.startswith("def_API"):
                            # Skip paragraphs that are only see cref tags (these belong in seealso section)
                            # Remove all <see cref tags and check if there's any content left
                            import re
                            text_without_refs = re.sub(r'<see cref="[^"]+"/>\s*', '', para_text).strip()
                            if text_without_refs:
                                remarks_parts.append(para_text)
                    elif has_paramlist:
                        paramlist = para.find("parameterlist")
                        if paramlist is not None:
                            remaining_text_parts = []
                            if paramlist.tail and paramlist.tail.strip():
                                remaining_text_parts.append(paramlist.tail.strip())
                            found_paramlist = False
                            for child in para:
                                if found_paramlist:
                                    # Stop at simplesect elements (they have their own sections)
                                    if child.tag == 'simplesect':
                                        break
                                    child_text = extract_text_from_element(child, convert_refs_to_see_cref=True)
                                    if child_text:
                                        remaining_text_parts.append(child_text)
                                    if child.tail and child.tail.strip():
                                        remaining_text_parts.append(child.tail.strip())
                                if child == paramlist:
                                    found_paramlist = True
                            remaining_text = ' '.join(remaining_text_parts)
                            if remaining_text and not remaining_text.startswith("def_API"):
                                remarks_parts.append(remaining_text)

            # Preconditions
            if detail_elem is not None:
                pre_sections = detail_elem.findall(".//simplesect[@kind='pre']")
                for pre_section in pre_sections:
                    pre_text = extract_text_from_element(pre_section, convert_refs_to_see_cref=True)
                    if pre_text:
                        remarks_parts.append(f"Precondition: {pre_text}")

            # Postconditions
            if detail_elem is not None:
                post_sections = detail_elem.findall(".//simplesect[@kind='post']")
                for post_section in post_sections:
                    post_text = extract_text_from_element(post_section, convert_refs_to_see_cref=True)
                    if post_text:
                        remarks_parts.append(f"Postcondition: {post_text}")

            # Warnings
            if detail_elem is not None:
                warning_sections = detail_elem.findall(".//simplesect[@kind='warning']")
                for warning_section in warning_sections:
                    warning_text = extract_text_from_element(warning_section, convert_refs_to_see_cref=True)
                    if warning_text:
                        remarks_parts.append(f"Warning: {warning_text}")

            # Remarks
            if detail_elem is not None:
                remark_sections = detail_elem.findall(".//simplesect[@kind='remark']")
                for remark_section in remark_sections:
                    remark_text = extract_text_from_element(remark_section, convert_refs_to_see_cref=True)
                    if remark_text:
                        remarks_parts.append(remark_text)

            # Output remarks section if we have content
            if remarks_parts:
                print("/// <remarks>")
                for remark in remarks_parts:
                    # Check if remark contains <code> blocks
                    if '<code>' in remark and '</code>' in remark:
                        # Split by code blocks and handle specially
                        parts = remark.split('<code>')
                        for i, part in enumerate(parts):
                            if i == 0:
                                # Text before first code block
                                if part.strip():
                                    print(f"/// {part}")
                            else:
                                # Part contains </code>
                                if '</code>' in part:
                                    code_part, after_code = part.split('</code>', 1)
                                    print("/// <code>")
                                    for line in code_part.strip().split('\n'):
                                        print(f"/// {line}")
                                    print("/// </code>")
                                    if after_code.strip():
                                        print(f"/// {after_code.strip()}")
                    else:
                        print(f"/// {remark}")
                print("/// </remarks>")

            # See also references
            if detail_elem is not None:
                see_sections = detail_elem.findall(".//simplesect[@kind='see']")
                if see_sections:
                    for see_section in see_sections:
                        refs = see_section.findall(".//ref")
                        for ref in refs:
                            ref_text = ref.text if ref.text else ""
                            if ref_text:
                                print(f'/// <seealso cref="{ref_text}"/>')

            return

    print(f"Function '{func_name}' not found")


def view_function_structured(xml_path: Path, func_name: str, show_brief: bool = False, show_params: bool = False, show_returns: bool = False, show_detail: bool = False, show_seealso: bool = False):
    """View a function's documentation in structured format."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this function
    for memberdef in root.findall(".//memberdef[@kind='function']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == func_name:
            print(f"Function: {func_name}")
            print("=" * 80)
            print()

            if show_brief:
                # Extract brief description
                brief_elem = memberdef.find("briefdescription")
                if brief_elem is not None:
                    brief_text = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)
                    if brief_text:
                        print("BRIEF:")
                        print(brief_text)
                        print()

            if show_detail:
                # Extract detailed description (paragraphs and text after parameterlist)
                detail_elem = memberdef.find("detaileddescription")
                if detail_elem is not None:
                    detail_paras = []

                    for para in detail_elem.findall("para"):
                        # Check if this para contains parameterlist or simplesect
                        has_paramlist = para.find("parameterlist") is not None
                        has_simplesect = para.find("simplesect") is not None

                        if not has_paramlist and not has_simplesect:
                            # Regular para without special sections
                            para_text = extract_text_from_element(para, convert_refs_to_see_cref=True)
                            if para_text and not para_text.startswith("def_API"):
                                detail_paras.append(para_text)
                        elif has_paramlist:
                            # Para contains parameterlist - extract text AFTER the parameterlist
                            # Get text that comes after the parameterlist element
                            paramlist = para.find("parameterlist")
                            if paramlist is not None:
                                # Build a list of all content after parameterlist
                                remaining_text_parts = []

                                # Add tail text of parameterlist if present
                                if paramlist.tail and paramlist.tail.strip():
                                    remaining_text_parts.append(paramlist.tail.strip())

                                # Get all elements that come after parameterlist in the para
                                found_paramlist = False
                                for child in para:
                                    if found_paramlist:
                                        # This element comes after parameterlist
                                        child_text = extract_text_from_element(child, convert_refs_to_see_cref=True)
                                        if child_text:
                                            remaining_text_parts.append(child_text)
                                        # Also add the tail of this child
                                        if child.tail and child.tail.strip():
                                            remaining_text_parts.append(child.tail.strip())
                                    if child == paramlist:
                                        found_paramlist = True

                                remaining_text = ' '.join(remaining_text_parts)
                                if remaining_text and not remaining_text.startswith("def_API"):
                                    detail_paras.append(remaining_text)

                    if detail_paras:
                        print("DETAIL:")
                        for para_text in detail_paras:
                            print(para_text)
                            print()

            if show_params:
                # Extract parameters from detaileddescription
                detail_elem = memberdef.find("detaileddescription")
                if detail_elem is not None:
                    # Find the parameterlist
                    param_list = detail_elem.find(".//parameterlist[@kind='param']")
                    if param_list is not None:
                        print("PARAMETERS:")
                        for param_item in param_list.findall("parameteritem"):
                            # Get parameter name
                            param_name_elem = param_item.find(".//parametername")
                            param_name = param_name_elem.text if param_name_elem is not None else "?"

                            # Get parameter description
                            param_desc_elem = param_item.find(".//parameterdescription")
                            param_desc = extract_text_from_element(param_desc_elem, convert_refs_to_see_cref=True) if param_desc_elem is not None else ""

                            print(f"  {param_name}:")
                            print(f"    {param_desc}")
                        print()

            if show_returns:
                # Extract return value documentation from detaileddescription
                detail_elem = memberdef.find("detaileddescription")
                if detail_elem is not None:
                    # Find simplesect with kind="return"
                    return_sections = detail_elem.findall(".//simplesect[@kind='return']")
                    if return_sections:
                        print("RETURNS:")
                        for return_section in return_sections:
                            return_text = extract_text_from_element(return_section, convert_refs_to_see_cref=True)
                            if return_text:
                                print(f"  {return_text}")
                        print()

            # Extract preconditions (always show if present)
            detail_elem = memberdef.find("detaileddescription")
            if detail_elem is not None:
                pre_sections = detail_elem.findall(".//simplesect[@kind='pre']")
                if pre_sections:
                    print("PRECONDITIONS:")
                    for pre_section in pre_sections:
                        pre_text = extract_text_from_element(pre_section, convert_refs_to_see_cref=True)
                        if pre_text:
                            print(f"  {pre_text}")
                    print()

            # Extract postconditions (always show if present)
            if detail_elem is not None:
                post_sections = detail_elem.findall(".//simplesect[@kind='post']")
                if post_sections:
                    print("POSTCONDITIONS:")
                    for post_section in post_sections:
                        post_text = extract_text_from_element(post_section, convert_refs_to_see_cref=True)
                        if post_text:
                            print(f"  {post_text}")
                    print()

            # Extract warnings (always show if present)
            if detail_elem is not None:
                warning_sections = detail_elem.findall(".//simplesect[@kind='warning']")
                if warning_sections:
                    print("WARNINGS:")
                    for warning_section in warning_sections:
                        warning_text = extract_text_from_element(warning_section, convert_refs_to_see_cref=True)
                        if warning_text:
                            print(f"  {warning_text}")
                    print()

            # Extract remarks (always show if present)
            if detail_elem is not None:
                remark_sections = detail_elem.findall(".//simplesect[@kind='remark']")
                if remark_sections:
                    print("REMARKS:")
                    for remark_section in remark_sections:
                        remark_text = extract_text_from_element(remark_section, convert_refs_to_see_cref=True)
                        if remark_text:
                            print(f"  {remark_text}")
                    print()

            if show_seealso:
                # Extract "see also" references from detaileddescription
                detail_elem = memberdef.find("detaileddescription")
                if detail_elem is not None:
                    # Find all simplesect elements with kind="see"
                    see_sections = detail_elem.findall(".//simplesect[@kind='see']")
                    if see_sections:
                        print("SEE ALSO:")
                        for see_section in see_sections:
                            # Extract all ref elements (references to other functions)
                            refs = see_section.findall(".//ref")
                            for ref in refs:
                                ref_text = ref.text if ref.text else ""
                                if ref_text:
                                    print(f"  - {ref_text}")
                        print()

            return

    print(f"Function '{func_name}' not found")


def view_function_raw(xml_path: Path, func_name: str):
    """View a function's documentation as raw XML."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this function
    for memberdef in root.findall(".//memberdef[@kind='function']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == func_name:
            print(f"Found function: {func_name}")
            print("=" * 80)
            print()

            # Pretty print the XML
            from xml.dom import minidom
            xml_string = ET.tostring(memberdef, encoding='unicode')
            dom = minidom.parseString(xml_string)
            pretty_xml = dom.toprettyxml(indent='  ')

            # Remove the XML declaration line
            lines = pretty_xml.split('\n')
            pretty_xml = '\n'.join(lines[1:])

            print(pretty_xml)
            return

    print(f"Function '{func_name}' not found")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python view_doxygen_function.py <function_name> [options]")
        print()
        print("Options:")
        print("  --brief      Show brief description")
        print("  --detail     Show detailed description")
        print("  --params     Show parameters")
        print("  --returns    Show return value documentation")
        print("  --seealso    Show 'see also' references")
        print("  --all        Show all structured information")
        print("  --dotnet     Generate .NET XML comments")
        print("  (no flags)   Show raw XML")
        print()
        print("Examples:")
        print("  python view_doxygen_function.py Z3_mk_int")
        print("  python view_doxygen_function.py Z3_mk_int --brief")
        print("  python view_doxygen_function.py Z3_mk_int --brief --params")
        print("  python view_doxygen_function.py Z3_mk_int --all")
        print("  python view_doxygen_function.py Z3_mk_int --dotnet")
        sys.exit(1)

    func_name = sys.argv[1]
    show_dotnet = "--dotnet" in sys.argv
    show_all = "--all" in sys.argv
    show_brief = "--brief" in sys.argv or show_all
    show_detail = "--detail" in sys.argv or show_all
    show_params = "--params" in sys.argv or show_all
    show_returns = "--returns" in sys.argv or show_all
    show_seealso = "--seealso" in sys.argv or show_all

    project_root = Path(__file__).parent.parent
    xml_path = project_root / ".cache" / "doxygen" / "xml" / "group__capi.xml"

    if show_dotnet:
        view_function_dotnet(xml_path, func_name)
    elif show_brief or show_detail or show_params or show_returns or show_seealso:
        view_function_structured(xml_path, func_name, show_brief=show_brief, show_params=show_params, show_returns=show_returns, show_detail=show_detail, show_seealso=show_seealso)
    else:
        view_function_raw(xml_path, func_name)
