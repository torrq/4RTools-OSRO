#!/bin/bash
#
# NAMESPACE TRANSFORMATION SCRIPT
# Systematically updates all namespaces for Brute Gaming Macros
#

set -e

echo "======================================================================"
echo "  BRUTE GAMING MACROS - NAMESPACE TRANSFORMATION"
echo "======================================================================"
echo ""

# Color codes
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Counter
total_files=0
updated_files=0

# Function to update namespace in a file
update_namespace() {
    local file=$1
    local old_ns=$2
    local new_ns=$3

    if grep -q "namespace ${old_ns}" "$file" 2>/dev/null; then
        sed -i "s|namespace ${old_ns}|namespace ${new_ns}|g" "$file"
        echo -e "${GREEN}✓${NC} Updated namespace in: $file"
        ((updated_files++))
        return 0
    fi
    return 1
}

# Function to update using statements
update_using() {
    local file=$1
    local old_ns=$2
    local new_ns=$3

    if grep -q "using ${old_ns}" "$file" 2>/dev/null; then
        sed -i "s|using ${old_ns}|using ${new_ns}|g" "$file"
        return 0
    fi
    return 1
}

echo -e "${BLUE}Phase 1: Updating namespace declarations${NC}"
echo ""

# Update Utils/ namespace declarations
echo "→ Utils/ directory..."
for file in Utils/*.cs; do
    ((total_files++))
    update_namespace "$file" "_4RTools.Utils" "BruteGamingMacros.Core.Utils"
done

# Update Model/ namespace declarations
echo "→ Model/ directory..."
for file in Model/*.cs; do
    ((total_files++))
    update_namespace "$file" "_4RTools.Model" "BruteGamingMacros.Core.Model"
done

# Update Forms/ namespace declarations
echo "→ Forms/ directory..."
for file in Forms/*.cs; do
    ((total_files++))
    update_namespace "$file" "_4RTools.Forms" "BruteGamingMacros.UI.Forms"
done

# Update Properties/ namespace
echo "→ Properties/ directory..."
for file in Properties/*.cs; do
    ((total_files++))
    update_namespace "$file" "_4RTools.Properties" "BruteGamingMacros.Properties"
done

# Update Resources/ namespace
echo "→ Resources/ directory..."
if [ -d "Resources/4RTools" ]; then
    for file in Resources/4RTools/*.cs; do
        ((total_files++))
        update_namespace "$file" "_4RTools.Resources._4RTools" "BruteGamingMacros.Resources.BruteGaming"
    done
fi

# Update Program.cs
echo "→ Program.cs..."
if [ -f "Program.cs" ]; then
    ((total_files++))
    update_namespace "Program.cs" "_4RTools" "BruteGamingMacros.Core"
fi

echo ""
echo -e "${BLUE}Phase 2: Updating using statements${NC}"
echo ""

# Update all using statements in all .cs files
find . -name "*.cs" ! -path "./bin/*" ! -path "./obj/*" -type f | while read file; do
    modified=0

    # Update each possible using statement
    if update_using "$file" "_4RTools.Utils" "BruteGamingMacros.Core.Utils"; then
        modified=1
    fi

    if update_using "$file" "_4RTools.Model" "BruteGamingMacros.Core.Model"; then
        modified=1
    fi

    if update_using "$file" "_4RTools.Forms" "BruteGamingMacros.UI.Forms"; then
        modified=1
    fi

    if update_using "$file" "_4RTools.Properties" "BruteGamingMacros.Properties"; then
        modified=1
    fi

    if update_using "$file" "_4RTools.Resources._4RTools" "BruteGamingMacros.Resources.BruteGaming"; then
        modified=1
    fi

    if update_using "$file" "_4RTools" "BruteGamingMacros.Core"; then
        modified=1
    fi

    if [ $modified -eq 1 ]; then
        echo -e "${GREEN}✓${NC} Updated using statements in: $file"
    fi
done

echo ""
echo "======================================================================"
echo -e "${GREEN}NAMESPACE TRANSFORMATION COMPLETE!${NC}"
echo "======================================================================"
echo "Total files processed: $total_files"
echo "Files with namespace updates: $updated_files"
echo ""
echo "Next steps:"
echo "  1. Review changes with: git diff"
echo "  2. Update AppConfig.cs manually"
echo "  3. Update AssemblyInfo.cs manually"
echo "  4. Rename project files"
echo ""
