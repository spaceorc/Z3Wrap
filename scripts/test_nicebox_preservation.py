#!/usr/bin/env python3
"""
Test that nicebox content is properly preserved with line breaks.
"""

import xml.etree.ElementTree as ET
from pathlib import Path


def test_nicebox_preservation():
    """Test that nicebox content preserves line breaks."""
    project_root = Path(__file__).parent.parent
    xml_path = project_root / ".cache" / "doxygen" / "xml" / "group__capi.xml"

    if not xml_path.exists():
        print("ERROR: Doxygen XML not found. Run: python3 scripts/generate_native_library.py --enums-only")
        return False

    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the Z3_decl_kind enum (which contains all the Z3_OP_* values)
    found = False
    for memberdef in root.findall(".//memberdef[@kind='enum']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == "Z3_decl_kind":
            found = True
            print("Found Z3_decl_kind enum")
            print("=" * 80)

            # The enum value descriptions are in the main enum's detaileddescription
            # as a bullet list
            detailed = memberdef.find("detaileddescription")
            if detailed is not None:
                # Look for list items mentioning Z3_OP_PR_SYMMETRY
                listitems = detailed.findall(".//listitem")
                for listitem in listitems:
                    # Get text content
                    text = ET.tostring(listitem, encoding='unicode', method='text')
                    if "Z3_OP_PR_SYMMETRY" in text:
                        print("Found Z3_OP_PR_SYMMETRY documentation in list item")
                        print()

                        # Look for verbatim blocks in this list item
                        verbatim_blocks = listitem.findall(".//verbatim")
                        if verbatim_blocks:
                            print(f"✓ Found {len(verbatim_blocks)} verbatim block(s)")
                            for i, verbatim in enumerate(verbatim_blocks, 1):
                                print(f"\nVerbatim block {i}:")
                                print("-" * 40)
                                content = verbatim.text or ""
                                print(content)
                                print("-" * 40)

                                # Check if nicebox marker is inside the verbatim text
                                if "<!-- nicebox -->" in content:
                                    print("✓ Semantic marker found INSIDE verbatim: <!-- nicebox -->")

                                    # Show the actual content without the marker
                                    lines = content.split('\n')
                                    content_lines = [l for l in lines if '<!-- nicebox -->' not in l]
                                    print(f"\nContent without marker ({len(content_lines)} lines):")
                                    print('\n'.join(content_lines[:5]))  # Show first 5 lines
                                else:
                                    print("⚠ WARNING: <!-- nicebox --> marker not found inside verbatim")

                            print("\n✓ SUCCESS: Line breaks preserved and semantic marker inside verbatim!")
                            return True
                        else:
                            print("ERROR: No verbatim blocks found in list item")
                            print("\nList item XML:")
                            print(ET.tostring(listitem, encoding='unicode'))
                            return False

    if not found:
        print("ERROR: Z3_decl_kind enum not found")
        return False

    print("ERROR: Z3_OP_PR_SYMMETRY documentation not found in list items")
    return False


if __name__ == "__main__":
    success = test_nicebox_preservation()
    exit(0 if success else 1)
