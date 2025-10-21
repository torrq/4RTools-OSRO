#!/bin/bash
#
# COMPREHENSIVE BACKUP SCRIPT
# Creates complete backup of 4RTools OSRO before Brute Gaming Macros transformation
# Author: Claude Code Analysis System
# Date: 2025-10-21
#

set -e  # Exit on error

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
BACKUP_ROOT="/home/user/BACKUP_ORIGINAL_4RTOOLS"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="${BACKUP_ROOT}/${TIMESTAMP}"
SOURCE_DIR="/home/user/4RTools-OSRO"

echo -e "${BLUE}═══════════════════════════════════════════════════════${NC}"
echo -e "${BLUE}     4RTOOLS OSRO - COMPREHENSIVE BACKUP SYSTEM        ${NC}"
echo -e "${BLUE}═══════════════════════════════════════════════════════${NC}"
echo ""

# Function to print section headers
print_section() {
    echo -e "${YELLOW}▶ $1${NC}"
}

# Function to print success
print_success() {
    echo -e "${GREEN}  ✓ $1${NC}"
}

# Function to print error
print_error() {
    echo -e "${RED}  ✗ $1${NC}"
}

# Create backup directory structure
print_section "Creating backup directory structure..."
mkdir -p "${BACKUP_DIR}/Source"
mkdir -p "${BACKUP_DIR}/Config"
mkdir -p "${BACKUP_DIR}/Documentation"
mkdir -p "${BACKUP_DIR}/Project_Files"
mkdir -p "${BACKUP_DIR}/Properties"
mkdir -p "${BACKUP_DIR}/Resources"
mkdir -p "${BACKUP_DIR}/Forms"
mkdir -p "${BACKUP_DIR}/Model"
mkdir -p "${BACKUP_DIR}/Utils"
mkdir -p "${BACKUP_DIR}/Git_Info"
print_success "Directory structure created"

# Backup all source files
print_section "Backing up source code files..."
cd "$SOURCE_DIR"

# Copy all C# files
find . -name "*.cs" -type f ! -path "./bin/*" ! -path "./obj/*" -exec cp --parents {} "${BACKUP_DIR}/Source/" \;
print_success "C# source files backed up"

# Copy all Designer files
find . -name "*.Designer.cs" -type f ! -path "./bin/*" ! -path "./obj/*" -exec cp --parents {} "${BACKUP_DIR}/Source/" \;
print_success "Designer files backed up"

# Copy all resource files
find . -name "*.resx" -type f ! -path "./bin/*" ! -path "./obj/*" -exec cp --parents {} "${BACKUP_DIR}/Source/" \;
print_success "Resource files backed up"

# Backup project files
print_section "Backing up project configuration..."
cp "OSRO Tools.csproj" "${BACKUP_DIR}/Project_Files/" 2>/dev/null || true
cp "OSROTools.sln" "${BACKUP_DIR}/Project_Files/" 2>/dev/null || true
cp "App.config" "${BACKUP_DIR}/Project_Files/" 2>/dev/null || true
cp "FodyWeavers.xml" "${BACKUP_DIR}/Project_Files/" 2>/dev/null || true
cp "packages.config" "${BACKUP_DIR}/Project_Files/" 2>/dev/null || true
print_success "Project files backed up"

# Backup Properties folder
print_section "Backing up Properties folder..."
if [ -d "Properties" ]; then
    cp -r Properties/* "${BACKUP_DIR}/Properties/" 2>/dev/null || true
    print_success "Properties folder backed up"
fi

# Backup Resources folder
print_section "Backing up Resources folder..."
if [ -d "Resources" ]; then
    cp -r Resources/* "${BACKUP_DIR}/Resources/" 2>/dev/null || true
    print_success "Resources folder backed up"
fi

# Backup documentation
print_section "Backing up documentation..."
cp README.md "${BACKUP_DIR}/Documentation/" 2>/dev/null || true
cp LICENSE "${BACKUP_DIR}/Documentation/" 2>/dev/null || true
cp BACKUP_SYSTEM.md "${BACKUP_DIR}/Documentation/" 2>/dev/null || true
print_success "Documentation backed up"

# Backup Git information
print_section "Saving Git repository state..."
git log -1 > "${BACKUP_DIR}/Git_Info/last_commit.txt" 2>/dev/null || true
git status > "${BACKUP_DIR}/Git_Info/git_status.txt" 2>/dev/null || true
git branch -v > "${BACKUP_DIR}/Git_Info/branches.txt" 2>/dev/null || true
git remote -v > "${BACKUP_DIR}/Git_Info/remotes.txt" 2>/dev/null || true
git diff > "${BACKUP_DIR}/Git_Info/uncommitted_changes.diff" 2>/dev/null || true
print_success "Git state saved"

# Create file inventory
print_section "Creating file inventory..."
find "${BACKUP_DIR}" -type f > "${BACKUP_DIR}/FILE_INVENTORY.txt"
FILE_COUNT=$(wc -l < "${BACKUP_DIR}/FILE_INVENTORY.txt")
print_success "Inventory created: ${FILE_COUNT} files backed up"

# Calculate backup size
BACKUP_SIZE=$(du -sh "${BACKUP_DIR}" | cut -f1)

# Create backup manifest
print_section "Creating backup manifest..."
cat > "${BACKUP_DIR}/MANIFEST.txt" << EOF
═══════════════════════════════════════════════════════
  4RTOOLS OSRO - BACKUP MANIFEST
═══════════════════════════════════════════════════════

Backup Date: $(date)
Timestamp: ${TIMESTAMP}
Source Directory: ${SOURCE_DIR}
Backup Directory: ${BACKUP_DIR}
Total Files: ${FILE_COUNT}
Total Size: ${BACKUP_SIZE}

Git Information:
----------------
Branch: $(git branch --show-current 2>/dev/null || echo "unknown")
Last Commit: $(git log -1 --oneline 2>/dev/null || echo "unknown")
Uncommitted Changes: $(git status --short | wc -l) files

Backup Contents:
----------------
✓ All C# source files (.cs)
✓ All Designer files (.Designer.cs)
✓ All resource files (.resx)
✓ Project configuration files
✓ Properties and AssemblyInfo
✓ Resources (icons, images, sounds)
✓ Documentation (README, LICENSE)
✓ Git repository state

Verification:
-------------
MD5 Checksum: $(find "${BACKUP_DIR}" -type f -exec md5sum {} \; | md5sum | cut -d' ' -f1)

═══════════════════════════════════════════════════════
EOF
print_success "Manifest created"

# Create restore script
print_section "Creating restore script..."
cat > "${BACKUP_DIR}/restore_from_backup.sh" << 'RESTORE_SCRIPT'
#!/bin/bash
#
# RESTORE SCRIPT
# Restores 4RTools OSRO from backup
#

set -e

echo "═══════════════════════════════════════════════════════"
echo "     4RTOOLS OSRO - RESTORATION SCRIPT                 "
echo "═══════════════════════════════════════════════════════"
echo ""

BACKUP_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TARGET_DIR="/home/user/4RTools-OSRO"

echo "⚠️  WARNING: This will overwrite files in ${TARGET_DIR}"
echo ""
read -p "Are you sure you want to proceed? (yes/no): " CONFIRM

if [ "$CONFIRM" != "yes" ]; then
    echo "Restoration cancelled."
    exit 0
fi

echo ""
echo "▶ Starting restoration..."

# Restore source files
echo "  → Restoring source files..."
cp -r "${BACKUP_DIR}/Source/." "${TARGET_DIR}/"

# Restore project files
echo "  → Restoring project files..."
cp "${BACKUP_DIR}/Project_Files/"* "${TARGET_DIR}/" 2>/dev/null || true

# Restore properties
echo "  → Restoring Properties folder..."
cp -r "${BACKUP_DIR}/Properties/." "${TARGET_DIR}/Properties/" 2>/dev/null || true

# Restore resources
echo "  → Restoring Resources folder..."
cp -r "${BACKUP_DIR}/Resources/." "${TARGET_DIR}/Resources/" 2>/dev/null || true

# Restore documentation
echo "  → Restoring documentation..."
cp "${BACKUP_DIR}/Documentation/"* "${TARGET_DIR}/" 2>/dev/null || true

echo ""
echo "✓ Restoration complete!"
echo ""
echo "Please verify the restored files:"
echo "  cd ${TARGET_DIR}"
echo "  git status"
echo ""
RESTORE_SCRIPT

chmod +x "${BACKUP_DIR}/restore_from_backup.sh"
print_success "Restore script created and made executable"

# Create symlink to latest backup
print_section "Creating symlink to latest backup..."
rm -f "${BACKUP_ROOT}/LATEST"
ln -s "${BACKUP_DIR}" "${BACKUP_ROOT}/LATEST"
print_success "Symlink created: ${BACKUP_ROOT}/LATEST"

# Final summary
echo ""
echo -e "${GREEN}═══════════════════════════════════════════════════════${NC}"
echo -e "${GREEN}     BACKUP COMPLETED SUCCESSFULLY!                    ${NC}"
echo -e "${GREEN}═══════════════════════════════════════════════════════${NC}"
echo ""
echo -e "${BLUE}Backup Location:${NC} ${BACKUP_DIR}"
echo -e "${BLUE}Total Files:${NC} ${FILE_COUNT}"
echo -e "${BLUE}Total Size:${NC} ${BACKUP_SIZE}"
echo ""
echo -e "${YELLOW}To restore from this backup:${NC}"
echo -e "  cd ${BACKUP_DIR}"
echo -e "  ./restore_from_backup.sh"
echo ""
echo -e "${YELLOW}Quick restore from latest:${NC}"
echo -e "  ${BACKUP_ROOT}/LATEST/restore_from_backup.sh"
echo ""
echo -e "${GREEN}✓ You can now safely proceed with the transformation!${NC}"
echo ""
