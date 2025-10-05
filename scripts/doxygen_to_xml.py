#!/usr/bin/env python3
"""
Doxygen to C# XML Documentation Generator

Converts parsed Doxygen tree structures into C# XML documentation comments.
"""

from typing import List, Union
from doxygen_parser import (
    DocComment, ParamDoc, ParagraphNode, BulletListNode, BulletItemNode,
    CodeBlockNode, NiceBoxNode, TextNode, InlineCodeNode, ReferenceNode
)


class XmlGenerator:
    """Generates C# XML documentation from parsed Doxygen trees."""

    def __init__(self, indent: str = "    "):
        self.indent = indent

    def generate(self, doc: DocComment) -> List[str]:
        """
        Generate C# XML documentation lines from a DocComment tree.

        Args:
            doc: Parsed documentation tree

        Returns:
            List of XML comment lines (including /// prefix)
        """
        lines = []

        # Summary (brief)
        if doc.brief:
            lines.append(f"{self.indent}/// <summary>")
            for node in doc.brief:
                lines.extend(self._generate_block_node(node))
            lines.append(f"{self.indent}/// </summary>")

        # Description (between brief and params) - add to summary if present
        # OR could be separate remarks section

        # Parameters
        for param_name, param_doc in doc.params.items():
            lines.extend(self._generate_param(param_doc))

        # Returns
        if doc.returns:
            lines.append(f"{self.indent}/// <returns>")
            for node in doc.returns:
                lines.extend(self._generate_block_node(node))
            lines.append(f"{self.indent}/// </returns>")

        # Remarks (body, preconditions, warnings, remarks combined)
        remarks_parts = []

        # Body paragraphs
        if doc.body:
            remarks_parts.extend(doc.body)

        # Preconditions
        for pre_nodes in doc.preconditions:
            # Add "Precondition: " prefix
            if pre_nodes:
                remarks_parts.append(TextNode("Precondition: "))
                remarks_parts.extend(pre_nodes)

        # Warnings
        for warn_nodes in doc.warnings:
            if warn_nodes:
                remarks_parts.append(TextNode("Warning: "))
                remarks_parts.extend(warn_nodes)

        # Remarks
        for remark_nodes in doc.remarks:
            remarks_parts.extend(remark_nodes)

        if remarks_parts:
            lines.append(f"{self.indent}/// <remarks>")
            for node in remarks_parts:
                lines.extend(self._generate_block_node(node))
            lines.append(f"{self.indent}/// </remarks>")

        # See also
        for sa_ref in doc.see_also:
            # TODO: Convert Z3_function_name to CSharp MethodName
            lines.append(f'{self.indent}/// <seealso cref="{sa_ref}"/>')

        return lines

    def _generate_param(self, param_doc: ParamDoc) -> List[str]:
        """Generate XML for a parameter."""
        lines = []

        # Check if single line or multiline
        is_single_line = len(param_doc.content) == 1 and isinstance(param_doc.content[0], ParagraphNode)

        if is_single_line:
            # Single line parameter
            para = param_doc.content[0]
            text = self._generate_inline_content(para.content)

            # Add ctype attribute if present
            ctype_attr = f' ctype="{param_doc.c_type}"' if param_doc.c_type else ''

            lines.append(f'{self.indent}/// <param name="{param_doc.name}"{ctype_attr}>{text}</param>')
        else:
            # Multiline parameter
            ctype_attr = f' ctype="{param_doc.c_type}"' if param_doc.c_type else ''
            lines.append(f'{self.indent}/// <param name="{param_doc.name}"{ctype_attr}>')

            for node in param_doc.content:
                lines.extend(self._generate_block_node(node))

            lines.append(f'{self.indent}/// </param>')

        return lines

    def _generate_block_node(self, node) -> List[str]:
        """Generate XML lines for a block-level node."""
        if isinstance(node, ParagraphNode):
            return self._generate_paragraph(node)
        elif isinstance(node, BulletListNode):
            return self._generate_bullet_list(node)
        elif isinstance(node, CodeBlockNode):
            return self._generate_code_block(node)
        elif isinstance(node, NiceBoxNode):
            return self._generate_nice_box(node)
        elif isinstance(node, TextNode):
            # Plain text node at block level - wrap in para
            return [f'{self.indent}/// {self._escape_xml(node.text)}']
        else:
            return [f'{self.indent}/// [Unknown node type: {type(node).__name__}]']

    def _generate_paragraph(self, para: ParagraphNode) -> List[str]:
        """Generate XML for a paragraph."""
        text = self._generate_inline_content(para.content)
        return [f'{self.indent}/// {text}']

    def _generate_bullet_list(self, bullet_list: BulletListNode) -> List[str]:
        """Generate XML for a bullet list."""
        lines = []
        lines.append(f'{self.indent}/// <list type="bullet">')

        for item in bullet_list.items:
            text = self._generate_inline_content(item.content)
            lines.append(f'{self.indent}/// <item><description>{text}</description></item>')

        lines.append(f'{self.indent}/// </list>')
        return lines

    def _generate_code_block(self, code_block: CodeBlockNode) -> List[str]:
        """Generate XML for a code block."""
        lines = []
        lines.append(f'{self.indent}/// <code>')

        for line in code_block.code.split('\n'):
            lines.append(f'{self.indent}/// {self._escape_xml(line)}')

        lines.append(f'{self.indent}/// </code>')
        return lines

    def _generate_nice_box(self, nice_box: NiceBoxNode) -> List[str]:
        """Generate XML for a nicebox (render as code with box drawing)."""
        lines = []

        # Find max width
        max_width = max(len(line) for line in nice_box.lines) if nice_box.lines else 0

        lines.append(f'{self.indent}/// <code>')
        lines.append(f'{self.indent}/// ╔{"═" * (max_width + 2)}╗')

        for line in nice_box.lines:
            padded = line.ljust(max_width)
            lines.append(f'{self.indent}/// ║ {self._escape_xml(padded)} ║')

        lines.append(f'{self.indent}/// ╚{"═" * (max_width + 2)}╝')
        lines.append(f'{self.indent}/// </code>')

        return lines

    def _generate_inline_content(self, content: List[Union[TextNode, InlineCodeNode, ReferenceNode]]) -> str:
        """Generate XML for inline content (text, code, references)."""
        parts = []

        for node in content:
            if isinstance(node, TextNode):
                parts.append(self._escape_xml(node.text))
            elif isinstance(node, InlineCodeNode):
                # Use <c> for inline code, not <code> (which is block-level)
                parts.append(f'<c>{self._escape_xml(node.code)}</c>')
            elif isinstance(node, ReferenceNode):
                # TODO: Convert Z3_name to proper cref
                parts.append(f'<see cref="{node.symbol}"/>')
            else:
                parts.append(f'[{type(node).__name__}]')

        return ''.join(parts)

    def _escape_xml(self, text: str) -> str:
        """Escape XML special characters."""
        return (text
                .replace('&', '&amp;')
                .replace('<', '&lt;')
                .replace('>', '&gt;')
                .replace('"', '&quot;'))


def main():
    """Test XML generation."""
    from doxygen_parser import DoxygenParser

    test = """
       \\brief Return numeral value, as a pair of 64 bit numbers if the representation fits.

       \\param c logical context.
       \\param a term.
       \\param num numerator.
       \\param den denominator.

       Return \\c true if the numeral value fits in 64 bit numerals, \\c false otherwise.

       Equivalent to \\c Z3_get_numeral_rational_int64 except that for unsupported expression.

       \\pre Z3_get_ast_kind(a) == Z3_NUMERAL_AST
    """

    parser = DoxygenParser()
    doc = parser.parse(test)

    # Add C types for testing
    if 'c' in doc.params:
        doc.params['c'].c_type = 'Z3_context'
    if 'a' in doc.params:
        doc.params['a'].c_type = 'Z3_ast'
    if 'num' in doc.params:
        doc.params['num'].c_type = 'int64_t*'
    if 'den' in doc.params:
        doc.params['den'].c_type = 'int64_t*'

    generator = XmlGenerator()
    xml_lines = generator.generate(doc)

    print("Generated XML:")
    print("=" * 60)
    for line in xml_lines:
        print(line)


if __name__ == '__main__':
    main()
