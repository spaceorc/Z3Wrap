#!/usr/bin/env python3
"""
Test that ccode tags are properly preserved with semantic markers.
"""

import xml.etree.ElementTree as ET
from pathlib import Path


def test_ccode_preservation():
    """Test that ccode content preserves semantic markers."""
    project_root = Path(__file__).parent.parent
    xml_path = project_root / ".cache" / "doxygen" / "xml" / "group__capi.xml"

    if not xml_path.exists():
        print("ERROR: Doxygen XML not found. Run: python3 scripts/generate_native_library.py --enums-only")
        return False

    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Search for the ‹ccode› marker in computeroutput tags
    xml_str = ET.tostring(root, encoding='unicode')

    import re
    # Look for computeroutput containing the ‹ccode› marker
    ccode_matches = re.findall(r'<computeroutput>‹ccode›(.*?)</computeroutput>', xml_str)

    print("Searching for ‹ccode› semantic markers inside <computeroutput> tags...")
    print("=" * 80)

    if ccode_matches:
        print(f"✓ Found {len(ccode_matches)} ‹ccode› marker(s) inside <computeroutput>")
        print("\nFirst 5 examples (content after marker):")
        for i, match in enumerate(ccode_matches[:5], 1):
            # Unescape HTML entities for display
            import html
            display = html.unescape(match)
            print(f"  {i}. {display}")
        print("\n✓ SUCCESS: Semantic ‹ccode› markers are preserved INSIDE content!")
        return True
    else:
        print("ERROR: No ‹ccode› markers found")
        return False


if __name__ == "__main__":
    success = test_ccode_preservation()
    exit(0 if success else 1)
