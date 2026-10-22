# Currently supported tool mods

Mods that have out of box support for various tools:

* van.datools
* TheSimpleDude42.ToolsOPlenty
* setosa.power.tools


Incompatible mods:

* Currently none. If you find anything, let me know on discord @1tzMara

<br />
<br />

# Important notice

This mod does not add any tools on its own. If you launch this mod without any tool mod installed, you might
find yourself not being able to do anything. See supported mods above or add your own.

# How to add custom support for a tool mod?

1) Add the desired mod to the LoadFolders.xml
2) Create your mod's folder in Mods/
3) Inside the newly created folder, create a Patches folder
4) Inside the Patches folder, create an XML file that containts your patch
5) Add this code to the new file.
```
    <Patch>
        <Operation Class="PatchOperationAddModExtension">
            <xpath>Defs/ThingDef[defName="YOUR_DEFNAME"]</xpath>
            <value>
                <li Class="ToolsAreNecessary.TANModExtension">
                    <canMine>true</canMine>
                    <canChop>true</canMine>
                </li>
            </value>
        </Operation>
    </Patch>
```
Replace "YOUR_DEFNAME" with the tool's defname
You can remove "canMine" or "canChop" to make the item incapable of doing that worktype.

If you need an example, check the existing patches

# How to add compatability for this mod?

To make this mod register your tool, add the "ToolsAreNecessary.TANModExtension" mod extension.

For tutorial on mod extensions, check the RW wiki:
https://rimworldwiki.com/wiki/Modding_Tutorials/DefModExtension


#### if you say i vibecoded this because i added a readme + comments im genuinely gonna slime you out.