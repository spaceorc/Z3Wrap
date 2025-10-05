#!/usr/bin/env python3
"""
View a specific enum's Doxygen XML documentation.
Usage: python view_doxygen_enum.py <enum_name> [--dotnet]
"""

import sys
import xml.etree.ElementTree as ET
from pathlib import Path
import re


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
        elif element.tag == 'verbatim':
            # This element is a code block (may contain nicebox marker)
            verbatim_text = element.text if element.text else ""
            # Check if this is a nicebox
            if '<!-- nicebox -->' in verbatim_text:
                # Remove the nicebox marker and format as nicebox
                verbatim_text = verbatim_text.replace('<!-- nicebox -->', '').strip()
                lines = verbatim_text.split('\n')
                # Find max width for box
                max_width = max(len(line) for line in lines) if lines else 0
                # Build nicebox
                box_lines = ['╔' + '═' * (max_width + 2) + '╗']
                for line in lines:
                    padded = line.ljust(max_width)
                    box_lines.append('║ ' + padded + ' ║')
                box_lines.append('╚' + '═' * (max_width + 2) + '╝')
                return '<code>\n' + '\n'.join(box_lines) + '\n</code>'
            else:
                # Regular verbatim - just wrap in code tags
                return f'<code>\n{verbatim_text}\n</code>'

    # Get text from this element and all children recursively
    text_parts = []
    if element.text:
        text_parts.append(element.text.strip())

    for child in element:
        # Check if this is a <ref> element (Doxygen reference to another function/type)
        if convert_refs_to_see_cref and child.tag == 'ref':
            ref_text = child.text if child.text else ""
            if ref_text:
                text_parts.append(f'<see cref="{ref_text}"/>')
            # Add tail text if present
            if child.tail:
                text_parts.append(child.tail.strip())
        # Check if this is a <computeroutput> element (inline code)
        elif convert_refs_to_see_cref and child.tag == 'computeroutput':
            code_text = child.text if child.text else ""
            if code_text:
                code_text = code_text.replace('‹ccode›', '')
                text_parts.append(f'<c>{code_text}</c>')
            # Add tail text if present
            if child.tail:
                text_parts.append(child.tail.strip())
        # Check if this is a <verbatim> element (code block)
        elif convert_refs_to_see_cref and child.tag == 'verbatim':
            verbatim_text = child.text if child.text else ""
            # Check if this is a nicebox
            if '<!-- nicebox -->' in verbatim_text:
                # Remove the nicebox marker and format as nicebox
                verbatim_text = verbatim_text.replace('<!-- nicebox -->', '').strip()
                lines = verbatim_text.split('\n')
                # Find max width for box
                max_width = max(len(line) for line in lines) if lines else 0
                # Build nicebox
                box_lines = ['╔' + '═' * (max_width + 2) + '╗']
                for line in lines:
                    padded = line.ljust(max_width)
                    box_lines.append('║ ' + padded + ' ║')
                box_lines.append('╚' + '═' * (max_width + 2) + '╝')
                text_parts.append('<code>\n' + '\n'.join(box_lines) + '\n</code>')
            else:
                # Regular verbatim - just wrap in code tags
                text_parts.append(f'<code>\n{verbatim_text}\n</code>')
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


def parse_enum_value_descriptions(detail_elem):
    """Parse itemizedlist in detaileddescription to extract per-value descriptions.

    Returns:
        dict: Mapping from enum value name to description text
    """
    value_descriptions = {}

    if detail_elem is None:
        return value_descriptions

    # Find itemizedlist elements
    for itemlist in detail_elem.findall('.//itemizedlist'):
        for listitem in itemlist.findall('listitem'):
            # Each listitem may have multiple para elements
            # The first para should start with the enum value name
            paras = listitem.findall('para')
            if not paras:
                continue

            # Extract text from first para to get the value name
            first_para = paras[0]
            first_para_text = first_para.text or ''

            # Pattern: "Z3_VALUE_NAME: description text" or "Z3_VALUE_NAME description"
            match = re.match(r'^(Z3_[A-Z_0-9]+):?\s*', first_para_text)
            if match:
                value_name = match.group(1)

                # Extract full content from ALL paras in this listitem
                all_para_texts = []
                for para in paras:
                    para_text = extract_text_from_element(para, convert_refs_to_see_cref=True)
                    if para_text:
                        all_para_texts.append(para_text)

                # Join all paragraphs
                full_text = ' '.join(all_para_texts)

                # Remove the value name prefix from the beginning
                description = re.sub(r'^' + re.escape(value_name) + r':?\s*', '', full_text).strip()

                if description:
                    value_descriptions[value_name] = description

    return value_descriptions


def view_enum_dotnet(xml_path: Path, enum_name: str):
    """View an enum's documentation as .NET XML comments."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this enum
    for memberdef in root.findall(".//memberdef[@kind='enum']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == enum_name:
            # Enum summary
            print(f"/// <summary>")

            # Brief description
            brief_elem = memberdef.find("briefdescription")
            if brief_elem is not None:
                brief_text = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)
                if brief_text:
                    print(f"/// {brief_text}")

            print(f"/// </summary>")

            # Parse value descriptions from detaileddescription
            detail_elem = memberdef.find("detaileddescription")
            value_descriptions = parse_enum_value_descriptions(detail_elem)

            # Print each enum value with its documentation
            print()
            for enumvalue in memberdef.findall("enumvalue"):
                value_name_elem = enumvalue.find("name")
                if value_name_elem is not None:
                    value_name = value_name_elem.text

                    # Get description for this value
                    description = value_descriptions.get(value_name, "")

                    if description:
                        print(f"/// <summary>")
                        print(f"/// {description}")
                        print(f"/// </summary>")

                    # Print placeholder for the enum value itself
                    print(f"{value_name},")
                    print()

            return

    print(f"Enum '{enum_name}' not found")


def view_enum_structured(xml_path: Path, enum_name: str):
    """View an enum's documentation in structured format."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this enum
    for memberdef in root.findall(".//memberdef[@kind='enum']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == enum_name:
            print(f"Enum: {enum_name}")
            print("=" * 80)
            print()

            # Extract brief description
            brief_elem = memberdef.find("briefdescription")
            if brief_elem is not None:
                brief_text = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)
                if brief_text:
                    print("BRIEF:")
                    print(brief_text)
                    print()

            # List all enum values
            print("VALUES:")
            for enumvalue in memberdef.findall("enumvalue"):
                value_name_elem = enumvalue.find("name")
                if value_name_elem is not None:
                    print(f"  - {value_name_elem.text}")
            print()

            # Extract detailed description
            detail_elem = memberdef.find("detaileddescription")
            if detail_elem is not None:
                value_descriptions = parse_enum_value_descriptions(detail_elem)

                if value_descriptions:
                    print("VALUE DESCRIPTIONS:")
                    for value_name, description in value_descriptions.items():
                        print(f"  {value_name}:")
                        print(f"    {description}")
                    print()

            # Extract see also references
            if detail_elem is not None:
                see_sections = detail_elem.findall(".//simplesect[@kind='see']")
                if see_sections:
                    print("SEE ALSO:")
                    for see_section in see_sections:
                        refs = see_section.findall(".//ref")
                        for ref in refs:
                            ref_text = ref.text if ref.text else ""
                            if ref_text:
                                print(f"  - {ref_text}")
                    print()

            return

    print(f"Enum '{enum_name}' not found")


def view_enum_raw(xml_path: Path, enum_name: str):
    """View an enum's documentation as raw XML."""
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this enum
    for memberdef in root.findall(".//memberdef[@kind='enum']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == enum_name:
            print(f"Found enum: {enum_name}")
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

    print(f"Enum '{enum_name}' not found")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python view_doxygen_enum.py <enum_name> [options]")
        print()
        print("Options:")
        print("  --structured Show structured format (brief, values, descriptions)")
        print("  --dotnet     Generate .NET XML comments")
        print("  (no flags)   Show raw XML")
        print()
        print("Examples:")
        print("  python view_doxygen_enum.py Z3_ast_print_mode")
        print("  python view_doxygen_enum.py Z3_ast_print_mode --structured")
        print("  python view_doxygen_enum.py Z3_ast_print_mode --dotnet")
        sys.exit(1)

    enum_name = sys.argv[1]
    show_dotnet = "--dotnet" in sys.argv
    show_structured = "--structured" in sys.argv

    project_root = Path(__file__).parent.parent
    xml_path = project_root / ".cache" / "doxygen" / "xml" / "group__capi.xml"

    if show_dotnet:
        view_enum_dotnet(xml_path, enum_name)
    elif show_structured:
        view_enum_structured(xml_path, enum_name)
    else:
        view_enum_raw(xml_path, enum_name)
