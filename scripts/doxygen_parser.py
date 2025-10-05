#!/usr/bin/env python3
"""
Doxygen Documentation Parser

Parses Doxygen-style comments from Z3 headers into a structured tree format.
This avoids the pitfalls of regex-based parsing by properly handling:
- Multiline parameter descriptions with continuation
- Body paragraphs after parameter sections
- Bullet lists, code blocks, and formatting
- Blank lines as section separators
"""

from dataclasses import dataclass, field
from typing import List, Dict, Optional, Union
from enum import Enum


class NodeType(Enum):
    """Types of documentation nodes."""
    TEXT = "text"
    PARAGRAPH = "paragraph"
    BULLET_LIST = "bullet_list"
    BULLET_ITEM = "bullet_item"
    CODE_BLOCK = "code_block"
    NICE_BOX = "nice_box"
    INLINE_CODE = "inline_code"
    REFERENCE = "reference"


@dataclass
class DocNode:
    """Base class for all documentation tree nodes."""
    node_type: NodeType


@dataclass
class TextNode(DocNode):
    """Plain text node."""
    text: str

    def __init__(self, text: str):
        super().__init__(NodeType.TEXT)
        self.text = text


@dataclass
class ParagraphNode(DocNode):
    """A paragraph containing text and inline formatting."""
    content: List[Union[TextNode, 'InlineCodeNode', 'ReferenceNode']] = field(default_factory=list)

    def __init__(self):
        super().__init__(NodeType.PARAGRAPH)
        self.content = []


@dataclass
class BulletItemNode(DocNode):
    """A single bullet list item."""
    content: List[Union[TextNode, 'InlineCodeNode', 'ReferenceNode']] = field(default_factory=list)

    def __init__(self):
        super().__init__(NodeType.BULLET_ITEM)
        self.content = []


@dataclass
class BulletListNode(DocNode):
    """A bullet list containing multiple items."""
    items: List[BulletItemNode] = field(default_factory=list)

    def __init__(self):
        super().__init__(NodeType.BULLET_LIST)
        self.items = []


@dataclass
class CodeBlockNode(DocNode):
    """A code block."""
    code: str

    def __init__(self, code: str):
        super().__init__(NodeType.CODE_BLOCK)
        self.code = code


@dataclass
class NiceBoxNode(DocNode):
    """A nice box (Z3 custom formatting)."""
    lines: List[str] = field(default_factory=list)

    def __init__(self):
        super().__init__(NodeType.NICE_BOX)
        self.lines = []


@dataclass
class InlineCodeNode(DocNode):
    r"""Inline code (\c or \ccode{})."""
    code: str

    def __init__(self, code: str):
        super().__init__(NodeType.INLINE_CODE)
        self.code = code


@dataclass
class ReferenceNode(DocNode):
    """Reference to another symbol (#Z3_name)."""
    symbol: str

    def __init__(self, symbol: str):
        super().__init__(NodeType.REFERENCE)
        self.symbol = symbol


@dataclass
class ParamDoc:
    """Documentation for a single parameter."""
    name: str
    c_type: Optional[str] = None  # Will be filled in post-processing
    content: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)


@dataclass
class DocComment:
    """Complete documentation comment structure."""
    brief: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    description: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    params: Dict[str, ParamDoc] = field(default_factory=dict)
    body: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    returns: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    preconditions: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    postconditions: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    warnings: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    remarks: List[Union[ParagraphNode, BulletListNode, CodeBlockNode]] = field(default_factory=list)
    see_also: List[str] = field(default_factory=list)


class DoxygenParser:
    """Parser for Doxygen-style documentation comments."""

    def __init__(self):
        self.lines = []
        self.current_line = 0

    def parse(self, comment_text: str) -> DocComment:
        """
        Parse a Doxygen comment block into a structured tree.

        Args:
            comment_text: The comment text (without /** */ markers)

        Returns:
            DocComment tree structure
        """
        # Preprocess: split into lines and clean up
        self.lines = self._preprocess_lines(comment_text)
        self.current_line = 0

        doc = DocComment()
        current_section = None  # Track which section we're in

        while self.current_line < len(self.lines):
            line = self.lines[self.current_line]

            # Skip empty lines (they're section separators, handled contextually)
            if not line.strip():
                self.current_line += 1
                continue

            ignore_tag = False
            
            # Check for Doxygen tags
            if line.strip().startswith('\\brief'):
                content = self._extract_tag_content('\\brief')
                doc.brief = self._parse_content_block(content)
                current_section = 'brief'

            elif line.strip().startswith('\\param'):
                param_name, param_content = self._extract_param()
                param_doc = ParamDoc(name=param_name)
                param_doc.content = self._parse_content_block(param_content)
                doc.params[param_name] = param_doc
                current_section = 'param'

            elif line.strip().startswith('\\returns'):
                content = self._extract_tag_content('\\returns')
                doc.returns = self._parse_content_block(content)
                current_section = 'returns'

            elif line.strip().startswith('\\pre'):
                content = self._extract_tag_content('\\pre')
                doc.preconditions.append(self._parse_content_block(content))
                current_section = 'pre'

            elif line.strip().startswith('\\post'):
                content = self._extract_tag_content('\\post')
                doc.postconditions.append(self._parse_content_block(content))
                current_section = 'post'

            elif line.strip().startswith('\\warning'):
                content = self._extract_tag_content('\\warning')
                doc.warnings.append(self._parse_content_block(content))
                current_section = 'warning'

            elif line.strip().startswith('\\remark'):
                content = self._extract_tag_content('\\remark')
                doc.remarks.append(self._parse_content_block(content))
                current_section = 'remark'

            elif line.strip().startswith('\\sa'):
                # Extract "see also" reference
                sa_line = line.strip()[3:].strip()  # Remove \sa
                if sa_line:
                    doc.see_also.append(sa_line)
                self.current_line += 1
                current_section = 'sa'

            elif line.strip().startswith('def_API'):
                # Skip internal Z3 metadata
                self.current_line += 1
                continue

            elif line.strip().startswith('\\'):
                # Unknown tag (like \ccode, \c, \nicebox) - treat as untagged content
                # Don't skip, let it fall through to untagged content handling
                ignore_tag = True
                pass

            if not (line.strip().startswith('\\brief') or
                    line.strip().startswith('\\param') or
                    line.strip().startswith('\\returns') or
                    line.strip().startswith('\\pre') or
                    line.strip().startswith('\\post') or
                    line.strip().startswith('\\warning') or
                    line.strip().startswith('\\remark') or
                    line.strip().startswith('\\sa') or
                    line.strip().startswith('def_API')):
                # Untagged content (or inline tags like \c, \ccode)
                content = self._extract_untagged_content(ignore_tag)
                parsed = self._parse_content_block(content)

                if current_section is None or current_section == 'brief':
                    # Before any params, goes to description
                    doc.description.extend(parsed)
                else:
                    # After params or other tags, goes to body
                    doc.body.extend(parsed)

        return doc

    def _preprocess_lines(self, comment_text: str) -> List[str]:
        """
        Preprocess comment text into clean lines.
        Removes leading asterisks and normalizes whitespace.
        """
        lines = comment_text.split('\n')
        cleaned = []

        for line in lines:
            # Remove leading whitespace and asterisk
            stripped = line.lstrip()
            if stripped.startswith('*'):
                stripped = stripped[1:].lstrip()
            cleaned.append(stripped)

        return cleaned

    def _extract_tag_content(self, tag: str) -> List[str]:
        """
        Extract content for a tag that ends at next tag or blank line.
        """
        content = []
        line = self.lines[self.current_line]

        # First line: remove tag and get rest
        first_content = line.strip()[len(tag):].strip()
        if first_content:
            content.append(first_content)

        self.current_line += 1

        # Continue until we hit a tag, blank line, or end
        while self.current_line < len(self.lines):
            line = self.lines[self.current_line]

            # Stop at blank line or next tag
            if not line.strip() or line.strip().startswith('\\'):
                break

            content.append(line)
            self.current_line += 1

        return content

    def _extract_param(self) -> tuple[str, List[str]]:
        """
        Extract parameter name and its content.
        Handles multiline descriptions with proper indentation detection.
        """
        line = self.lines[self.current_line]

        # Parse: \param name description...
        parts = line.strip()[6:].strip().split(None, 1)  # Remove \param
        param_name = parts[0]
        content = [parts[1]] if len(parts) > 1 else []

        self.current_line += 1

        # Simple approach: continue until we hit a blank line or next tag
        # Multiline params will have indented continuation lines
        while self.current_line < len(self.lines):
            line = self.lines[self.current_line]

            # Stop at blank line or next tag
            if not line.strip() or line.strip().startswith('\\'):
                break

            # Check if line is indented (continuation of param)
            # Continuation lines have leading whitespace
            if line and line[0] in ' \t':
                content.append(line.strip())
                self.current_line += 1
            else:
                # Not indented - stop here
                break

        return param_name, content

    def _extract_untagged_content(self, ignore_tag: bool) -> List[str]:
        """
        Extract untagged content until we hit a tag or end.
        """
        content = []

        while self.current_line < len(self.lines):
            line = self.lines[self.current_line]

            # Stop at tag
            if line.strip().startswith('\\') and not ignore_tag:
                break
                
            ignore_tag = False

            # Include even blank lines (for paragraph separation)
            content.append(line if line.strip() else '')
            self.current_line += 1

        return content

    def _parse_content_block(self, lines: List[str]) -> List[Union[ParagraphNode, BulletListNode, CodeBlockNode]]:
        """
        Parse a block of content lines into structured nodes.
        Handles paragraphs, bullet lists, code blocks, etc.
        """
        nodes = []
        i = 0

        while i < len(lines):
            line = lines[i]

            # Skip blank lines between blocks
            if not line.strip():
                i += 1
                continue

            # Check for bullet list
            if line.strip().startswith('- '):
                bullet_list, consumed = self._parse_bullet_list(lines[i:])
                nodes.append(bullet_list)
                i += consumed

            # Check for code block
            elif '\\code' in line:
                code_block, consumed = self._parse_code_block(lines[i:])
                nodes.append(code_block)
                i += consumed

            # Check for nicebox
            elif '\\nicebox{' in line:
                nice_box, consumed = self._parse_nice_box(lines[i:])
                nodes.append(nice_box)
                i += consumed

            # Regular paragraph
            else:
                paragraph, consumed = self._parse_paragraph(lines[i:])
                nodes.append(paragraph)
                i += consumed

        return nodes

    def _parse_paragraph(self, lines: List[str]) -> tuple[ParagraphNode, int]:
        """
        Parse a paragraph (text until blank line or special construct).
        """
        para = ParagraphNode()
        text_parts = []
        i = 0

        while i < len(lines):
            line = lines[i]

            # Stop at blank line or special constructs
            if not line.strip() or line.strip().startswith('- ') or '\\code' in line or '\\nicebox{' in line:
                break

            text_parts.append(line.strip())
            i += 1

        # Join and parse inline formatting
        full_text = ' '.join(text_parts)
        para.content = self._parse_inline_formatting(full_text)

        return para, i

    def _parse_bullet_list(self, lines: List[str]) -> tuple[BulletListNode, int]:
        """
        Parse a bullet list.
        """
        bullet_list = BulletListNode()
        i = 0

        while i < len(lines):
            line = lines[i]

            # Check if this is a new bullet item
            if line.strip().startswith('- '):
                # Extract bullet content (remove leading "- ")
                item_text = line.strip()[2:]

                # Look for continuation lines (indented)
                i += 1
                while i < len(lines):
                    next_line = lines[i]
                    # Stop at blank, new bullet, or tag
                    if not next_line.strip() or next_line.strip().startswith('- ') or next_line.strip().startswith('\\'):
                        break
                    # Continuation line (indented)
                    if next_line and next_line[0] in ' \t':
                        item_text += ' ' + next_line.strip()
                        i += 1
                    else:
                        break

                # Create bullet item
                item = BulletItemNode()
                item.content = self._parse_inline_formatting(item_text)
                bullet_list.items.append(item)
                # Note: i was already incremented in the inner loop
            else:
                # Not a bullet item anymore
                break

        return bullet_list, i

    def _parse_code_block(self, lines: List[str]) -> tuple[CodeBlockNode, int]:
        """
        Parse a code block between \\code and \\endcode.
        """
        code_lines = []
        i = 0
        in_code = False

        for line in lines:
            if '\\code' in line:
                in_code = True
                # Check if there's content after \code on same line
                after_code = line.split('\\code', 1)[1]
                if after_code.strip():
                    code_lines.append(after_code)
                i += 1
                continue

            if '\\endcode' in line:
                # Check if there's content before \endcode
                before_end = line.split('\\endcode', 1)[0]
                if before_end.strip():
                    code_lines.append(before_end)
                i += 1
                break

            if in_code:
                code_lines.append(line)

            i += 1

        code_text = '\n'.join(code_lines)
        return CodeBlockNode(code_text), i

    def _parse_nice_box(self, lines: List[str]) -> tuple[NiceBoxNode, int]:
        """
        Parse a nicebox (Z3 custom formatting).
        """
        nice_box = NiceBoxNode()
        i = 0
        in_box = False

        for line in lines:
            if '\\nicebox{' in line:
                in_box = True
                i += 1
                continue

            if in_box and '}' in line:
                i += 1
                break

            if in_box:
                nice_box.lines.append(line)

            i += 1

        return nice_box, i

    def _parse_inline_formatting(self, text: str) -> List[Union[TextNode, InlineCodeNode, ReferenceNode]]:
        """
        Parse inline formatting like \\c, \\ccode{}, and #references.
        For now, returns simple text nodes. TODO: Implement proper parsing.
        """
        # TODO: Implement proper inline parsing
        # For now, just return the text as-is
        return [TextNode(text)]


def main():
    """Test the parser with examples."""
    
    # Test case 1: Simple function with params and body
    test1 = """
       \\brief Return numeral value, as a pair of 64 bit numbers if the representation fits.

       \\param c logical context.
       \\param a term.
       \\param num numerator.
       \\param den denominator.

       Return \\c true if the numeral value fits in 64 bit numerals, \\c false otherwise.

       Equivalent to \\c Z3_get_numeral_rational_int64 except that for unsupported expression arguments \\c Z3_get_numeral_small signals an error while \\c Z3_get_numeral_rational_int64 returns \\c false.

       \\pre Z3_get_ast_kind(a) == Z3_NUMERAL_AST

       def_API('Z3_get_numeral_small', BOOL, (_in(CONTEXT), _in(AST), _out(INT64), _out(INT64)))
    """

    # Test case 3: Z3_mk_config with \ccode and bullet list
    test3 = r"""
       \brief Array read.
       The argument \c a is the array and \c i is the index of the array that gets read.

       The node \c a must have an array sort \ccode{[domain -> range]},
       and \c i must have the sort \c domain.
       The sort of the result is \c range.

       \sa Z3_mk_array_sort
       \sa Z3_mk_store

       def_API('Z3_mk_select', AST, (_in(CONTEXT), _in(AST), _in(AST)))
    """

    parser = DoxygenParser()
    doc = parser.parse(test3)
    
    print(doc)

    print("Test 1: Simple function")
    print("=" * 60)
    print(f"Brief: {len(doc.brief)} nodes")
    print(f"Params: {list(doc.params.keys())}")
    print(f"Body: {len(doc.body)} nodes")
    print(f"Preconditions: {len(doc.preconditions)}")
    print()
    
    return 

    # Test case 2: Multiline param
    test2 = """
       \\brief Create a constructor.

       \\param c logical context.
       \\param name constructor name.
       \\param sort_refs reference to datatype sort that is an argument to the constructor; if the corresponding
                        sort reference is 0, then the value in sort_refs should be an index referring to
                        one of the recursive datatypes that is declared.

       \\sa Z3_del_constructor
       \\sa Z3_mk_constructor_list
    """

    doc2 = parser.parse(test2)

    print("Test 2: Multiline param")
    print("=" * 60)
    print(f"Brief: {len(doc2.brief)} nodes")
    print(f"Params: {list(doc2.params.keys())}")
    if 'sort_refs' in doc2.params:
        print(f"sort_refs content: {len(doc2.params['sort_refs'].content)} nodes")
    print(f"See also: {doc2.see_also}")
    print()


if __name__ == '__main__':
    main()
