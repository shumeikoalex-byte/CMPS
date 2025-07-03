import os
import re
from pathlib import Path

# Directory containing SQL table definitions
TABLE_DIR = Path('dbo/BSharpProtoype/BSharp/dbo/Tables')

# Map SQL Server data types to dbml types
def convert_type(sql_type):
    t = sql_type.strip().upper()
    if t.startswith('NVARCHAR'):  # NVARCHAR (255)
        return 'varchar' + t[len('NVARCHAR'):].lower()
    if t.startswith('NCHAR'):
        return 'char' + t[len('NCHAR'):].lower()
    if t.startswith('DATETIMEOFFSET'):
        return 'datetimeoffset'
    if t.startswith('INT'):
        return 'int'
    if t.startswith('BIT'):
        return 'bit'
    if t.startswith('DECIMAL'):
        return 'decimal' + t[len('DECIMAL'):].lower()
    if t.startswith('FLOAT'):
        return 'float'
    if t.startswith('HIERARCHYID'):
        return None
    return t.lower()

# Parse a SQL file and return table dict
create_table_re = re.compile(r'CREATE\s+TABLE\s+\[dbo\]\.[\[](?P<name>[^\]]+)\]\s*\((?P<body>.*)\)\s*;', re.I | re.S)

fk_re = re.compile(r'FOREIGN\s+KEY\s*\(\[([^\]]+)\]\)\s+REFERENCES\s+\[dbo\]\.[\[]([^\]]+)\]\s*\(\[([^\]]+)\]\)', re.I)

def parse_sql_file(path):
    text = Path(path).read_text(encoding='utf-8-sig')
    lines = []
    for line in text.splitlines():
        line = line.split('--')[0]
        line = line.strip()
        if not line or line.upper() == 'GO':
            continue
        lines.append(line)
    cleaned = ' '.join(lines)
    m = create_table_re.search(cleaned)
    if not m:
        return None
    table_name = m.group('name')
    body = m.group('body')

    # split body by commas not inside parentheses
    parts = []
    buf = ''
    level = 0
    for ch in body:
        if ch == '(':
            level += 1
        elif ch == ')':
            level -= 1
        if ch == ',' and level == 0:
            if buf.strip():
                parts.append(buf.strip())
            buf = ''
        else:
            buf += ch
    if buf.strip():
        parts.append(buf.strip())

    columns = []
    fks = []
    comments = []
    for part in parts:
        if part.upper().startswith('CONSTRAINT'):
            m = fk_re.search(part)
            if m:
                columns_fk, ref_table, ref_col = m.groups()
                fks.append((columns_fk, ref_table, ref_col))
            continue
        m = re.match(r'\[([^\]]+)\]\s+([^\s]+(?:\s*\([^\)]*\))?)\s*(.*)', part)
        if not m:
            continue
        col_name, col_type, rest = m.groups()
        dbml_type = convert_type(col_type)
        if dbml_type is None:
            comments.append(f'// {col_name} {col_type} {rest}'.strip())
            continue
        attrs = []
        if 'PRIMARY KEY' in rest.upper():
            attrs.append('pk')
        if 'NOT NULL' in rest.upper():
            attrs.append('not null')
        default_match = re.search(r'DEFAULT\s+(.+)', rest, re.I)
        if default_match:
            default_val = default_match.group(1).strip().rstrip(',')
            attrs.append(f'default: {default_val}')
        columns.append((col_name, dbml_type, attrs))

    return {'name': table_name, 'columns': columns, 'fks': fks, 'comments': comments}


def generate_dbml():
    tables = []
    for path in sorted(TABLE_DIR.glob('*.sql')):
        tbl = parse_sql_file(path)
        if tbl:
            tables.append(tbl)
    lines = []
    for t in tables:
        lines.append(f'Table {t["name"]} {{')
        for col_name, col_type, attrs in t['columns']:
            attr_text = ''
            if attrs:
                attr_text = ' [' + ', '.join(attrs) + ']'
            lines.append(f'  {col_name} {col_type}{attr_text}')
        for c in t['comments']:
            lines.append(f'  {c}')
        lines.append("}")
    # add references
    for t in tables:
        for col, ref_table, ref_col in t['fks']:
            lines.append(f'Ref: {t["name"]}.{col} > {ref_table}.{ref_col}')
    return '\n'.join(lines)

if __name__ == "__main__":
    dbml = generate_dbml()
    Path('schema.dbml').write_text(dbml, encoding='utf-8')
    print('DBML generated to schema.dbml')
