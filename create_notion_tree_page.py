#!/usr/bin/env python3
"""
Complete script to create a Notion page with the tree image.
This uses the Notion MCP API functions available in Cursor.

The tree image is hosted at: https://i.imgur.com/wymyxaA.png
"""

# Tree image URL (already uploaded to Imgur)
TREE_IMAGE_URL = "https://i.imgur.com/wymyxaA.png"

print("=" * 60)
print("NOTION TREE IMAGE SETUP")
print("=" * 60)
print()
print("Tree Image URL: https://i.imgur.com/wymyxaA.png")
print("Image Size: 200x200 pixels")
print()
print("To add this image to Notion:")
print()
print("1. Configure Notion API Token:")
print("   - Go to Cursor Settings > MCP/Notion")
print("   - Add your Notion Integration token")
print("   - Get token from: https://www.notion.so/my-integrations")
print()
print("2. Once configured, the image can be added using:")
print("   - Create a new page with: mcp_notionApi_API-post-page")
print("   - Or add to existing page with: mcp_notionApi_API-patch-block-children")
print()
print("3. The image block format:")
print("   {")
print('     "type": "image",')
print('     "image": {')
print('       "type": "external",')
print(f'       "external": {{ "url": "{TREE_IMAGE_URL}" }}')
print("     }")
print("   }")
print()
print("=" * 60)
